namespace ExaminationSystem.Application.Common.Interfaces;

/// <summary>
/// Sliding-window rate limiter backed by Redis atomic INCR.
/// Two independent budgets are tracked:
///  - Send budget   : how many times an identifier may request a new OTP.
///  - Verify budget : how many failed verification attempts are allowed.
/// </summary>
public interface IRateLimiterService
{
    /// <summary>
    /// Returns <c>true</c> if the action is within the allowed budget.
    /// The counter resets automatically after <paramref name="window"/> elapses (TTL-based).
    /// </summary>
    /// <param name="key">
    /// A unique Redis key, e.g. <c>ratelimit:send:{email}</c> or <c>ratelimit:send:ip:{ip}</c>.
    /// </param>
    /// <param name="maxRequests">Maximum calls allowed within <paramref name="window"/>.</param>
    /// <param name="window">Sliding window duration.</param>
    Task<bool> IsAllowedAsync(
        string key,
        int maxRequests,
        TimeSpan window,
        CancellationToken ct = default);

    /// <summary>
    /// Increments a persistent counter (no TTL reset).
    /// Used to track failed verify attempts that outlive the OTP TTL.
    /// Returns the new counter value.
    /// </summary>
    Task<long> IncrementAsync(string key, CancellationToken ct = default);

    /// <summary>Reads the current counter value without modifying it.</summary>
    Task<long> GetCounterAsync(string key, CancellationToken ct = default);

    /// <summary>Deletes a counter key (e.g. after a successful verification).</summary>
    Task ResetAsync(string key, CancellationToken ct = default);
}
