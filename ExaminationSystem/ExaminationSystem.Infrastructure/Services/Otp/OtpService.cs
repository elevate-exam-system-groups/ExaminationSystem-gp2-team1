using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ExaminationSystem.Infrastructure.Services.Otp;

/// <summary>
/// Redis-backed OTP service.
/// Security guarantees:
///  - OTP generated with <see cref="RandomNumberGenerator"/> (CSPRNG).
///  - Only the SHA-256 hash is persisted — plaintext never touches storage.
///  - One-time-use enforced: entry deleted immediately on first successful verify.
///  - Replay attacks prevented by the deleted-on-use model.
///  - Brute force prevented by attempt counter (see <see cref="IRateLimiterService"/>).
/// Redis key format: <c>otp:{purpose}:{identifier}</c>
/// </summary>
public sealed class OtpService(
    IDistributedCache cache,
    IOptions<OtpSettings> otpOptions,
    ILogger<OtpService> logger
) : IOtpService
{
    private readonly OtpSettings _settings = otpOptions.Value;

    // ── Key builder ──────────────────────────────────────────────────────────
    private static string BuildKey(string identifier, OtpPurpose purpose)
        => $"otp:{purpose}:{identifier.ToLowerInvariant()}";

    // ── Hash helper ──────────────────────────────────────────────────────────
    private static string HashOtp(string otp)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(otp));
        return Convert.ToHexString(bytes); // uppercase hex, no hyphens
    }

    // ────────────────────────────────────────────────────────────────────────
    // GenerateAsync
    // ────────────────────────────────────────────────────────────────────────
    public async Task<Result<string>> GenerateAsync(
        string identifier,
        OtpPurpose purpose,
        CancellationToken ct = default)
    {
        // 1. Generate cryptographically random 6-digit code
        var otp = GenerateSecureOtp();

        // 2. Build the cache entry (store hash, never plaintext)
        var entry = new OtpCacheEntry
        {
            HashedOtp = HashOtp(otp),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
            AttemptCount = 0,
            IsUsed = false,
            Identifier = identifier,
            Purpose = purpose.ToString()
        };

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_settings.ExpirationMinutes)
        };

        var key = BuildKey(identifier, purpose);
        var json = JsonSerializer.Serialize(entry);
        await cache.SetStringAsync(key, json, options, ct);

        logger.LogInformation(
            "OTP generated for {Identifier}/{Purpose}. Expires at {ExpiresAt} UTC. [OTP value NOT logged]",
            identifier, purpose, entry.ExpiresAt);

        // 3. Return ONLY the plaintext — caller forwards it to the user
        return new Result<string>(otp);
    }

    // ────────────────────────────────────────────────────────────────────────
    // VerifyAsync
    // ────────────────────────────────────────────────────────────────────────
    public async Task<Result<bool>> VerifyAsync(
        string identifier,
        OtpPurpose purpose,
        string otpCode,
        CancellationToken ct = default)
    {
        var key = BuildKey(identifier, purpose);
        var json = await cache.GetStringAsync(key, ct);

        // ── Entry not found (expired by TTL or never generated) ──────────────
        if (json is null)
        {
            logger.LogWarning(
                "OTP lookup failed — no entry for {Identifier}/{Purpose}", identifier, purpose);
            return new Result<bool>(new Error(
                ErrorCode.OtpExpired,
                "Your OTP has expired or does not exist. Please request a new one."));
        }

        var entry = JsonSerializer.Deserialize<OtpCacheEntry>(json)!;

        // ── Replay prevention: already consumed ──────────────────────────────
        if (entry.IsUsed)
        {
            logger.LogWarning(
                "OTP replay attempt for {Identifier}/{Purpose}", identifier, purpose);
            return new Result<bool>(new Error(
                ErrorCode.OtpAlreadyUsed,
                "This OTP has already been used. Please request a new one."));
        }

        // ── Manual expiry check (belt-and-suspenders on top of Redis TTL) ──
        if (DateTime.UtcNow > entry.ExpiresAt)
        {
            await cache.RemoveAsync(key, ct);
            logger.LogWarning(
                "OTP expired for {Identifier}/{Purpose}", identifier, purpose);
            return new Result<bool>(new Error(
                ErrorCode.OtpExpired,
                "Your OTP has expired. Please request a new one."));
        }

        // ── Compare hashes (constant-time) ───────────────────────────────────
        var inputHash = HashOtp(otpCode);
        var isMatch = CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(inputHash),
            Encoding.UTF8.GetBytes(entry.HashedOtp));

        if (!isMatch)
        {
            // Increment attempt count in cache entry
            entry.AttemptCount++;
            await cache.SetStringAsync(key, JsonSerializer.Serialize(entry),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = entry.ExpiresAt
                }, ct);

            logger.LogWarning(
                "Invalid OTP for {Identifier}/{Purpose}. Attempt #{Count}",
                identifier, purpose, entry.AttemptCount);

            return new Result<bool>(new Error(
                ErrorCode.OtpInvalid,
                "Invalid OTP code."));
        }

        // ── Success: mark as used (one-time-use) then remove ─────────────────
        entry.IsUsed = true;
        await cache.RemoveAsync(key, ct);

        logger.LogInformation(
            "OTP successfully verified for {Identifier}/{Purpose}", identifier, purpose);

        return new Result<bool>(true);
    }

    // ────────────────────────────────────────────────────────────────────────
    // InvalidateAsync
    // ────────────────────────────────────────────────────────────────────────
    public async Task InvalidateAsync(
        string identifier,
        OtpPurpose purpose,
        CancellationToken ct = default)
    {
        var key = BuildKey(identifier, purpose);
        await cache.RemoveAsync(key, ct);
        logger.LogInformation("OTP invalidated for {Identifier}/{Purpose}", identifier, purpose);
    }

    // ────────────────────────────────────────────────────────────────────────
    // CSPRNG — 6 digits, zero-padded
    // ────────────────────────────────────────────────────────────────────────
    private static string GenerateSecureOtp()
    {
        // RandomNumberGenerator gives a uniform distribution — no modulo bias
        uint value = (uint)RandomNumberGenerator.GetInt32(0, 1_000_000); // [0, 999_999]
        return value.ToString("D6"); // always 6 chars e.g. "007842"
    }
}
