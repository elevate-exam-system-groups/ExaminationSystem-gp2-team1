using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;

namespace ExaminationSystem.Application.Common.Interfaces;

/// <summary>
/// Core OTP operations: generate, verify, and invalidate.
/// Implementations must:
/// - Use a cryptographically random 6-digit code.
/// - Store only the SHA-256 hash (never the plaintext).
/// - Enforce expiry, one-time-use, and attempt counting.
/// </summary>
public interface IOtpService
{
    /// <summary>
    /// Creates a new OTP for <paramref name="identifier"/> scoped to <paramref name="purpose"/>,
    /// persists the hash in Redis with a TTL, and returns the plaintext code
    /// (to be forwarded to the user via email / SMS — never stored).
    /// </summary>
    Task<Result<string>> GenerateAsync(
        string identifier,
        OtpPurpose purpose,
        CancellationToken ct = default);

    /// <summary>
    /// Verifies the provided <paramref name="otpCode"/> against the stored hash.
    /// Increments the attempt counter on failure.
    /// Invalidates (deletes) the Redis entry on success to prevent replay attacks.
    /// </summary>
    Task<Result<bool>> VerifyAsync(
        string identifier,
        OtpPurpose purpose,
        string otpCode,
        CancellationToken ct = default);

    /// <summary>
    /// Manually removes the OTP entry from Redis
    /// (e.g. after a successful verify-then-activate flow).
    /// </summary>
    Task InvalidateAsync(
        string identifier,
        OtpPurpose purpose,
        CancellationToken ct = default);
}
