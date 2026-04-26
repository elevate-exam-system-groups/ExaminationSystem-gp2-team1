using ExaminationSystem.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ExaminationSystem.Infrastructure.Services.Otp;

/// <summary>
/// Redis-backed rate limiter using atomic INCR + EXPIRE.
/// Two strategies:
///  1. <see cref="IsAllowedAsync"/> — sliding window via TTL-keyed counter
///     (resets the whole window when the key expires). Used for send/resend.
///  2. <see cref="IncrementAsync"/> — persistent counter (no auto-expiry).
///     Used for verify attempts that must outlive the OTP TTL.
/// </summary>
public sealed class RateLimiterService(
    IConnectionMultiplexer redis,
    ILogger<RateLimiterService> logger
) : IRateLimiterService
{
    private readonly IDatabase _db = redis.GetDatabase();

    /// <inheritdoc/>
    public async Task<bool> IsAllowedAsync(
        string key,
        int maxRequests,
        TimeSpan window,
        CancellationToken ct = default)
    {
        // Atomic INCR: returns new count after increment
        var count = await _db.StringIncrementAsync(key);

        if (count == 1)
        {
            // First request in the window — set the expiry
            await _db.KeyExpireAsync(key, window);
        }

        if (count > maxRequests)
        {
            logger.LogWarning(
                "Rate limit exceeded for key '{Key}'. Count={Count}, Max={Max}",
                key, count, maxRequests);
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<long> IncrementAsync(string key, CancellationToken ct = default)
        => await _db.StringIncrementAsync(key);

    /// <inheritdoc/>
    public async Task<long> GetCounterAsync(string key, CancellationToken ct = default)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue && long.TryParse((string?)value, out var count) ? count : 0;
    }

    /// <inheritdoc/>
    public async Task ResetAsync(string key, CancellationToken ct = default)
        => await _db.KeyDeleteAsync(key);
}
