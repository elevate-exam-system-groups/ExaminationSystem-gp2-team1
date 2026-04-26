namespace ExaminationSystem.Application.Common.Models;

/// <summary>
/// Strongly-typed configuration bound from appsettings.json "OtpSettings" section.
/// </summary>
public sealed class OtpSettings
{
    /// <summary>How many minutes before an OTP expires (default: 10).</summary>
    public int ExpirationMinutes { get; init; } = 10;

    /// <summary>Maximum failed verify attempts before the OTP is locked (default: 5).</summary>
    public int MaxVerifyAttempts { get; init; } = 68;

    /// <summary>Maximum OTP send/resend requests per window per identifier (default: 3).</summary>
    public int MaxSendRequestsPerWindow { get; init; } = 10;

    /// <summary>Sliding window duration in minutes for send rate-limiting (default: 60 minutes / 1 hour).</summary>
    public int SendWindowMinutes { get; init; } = 6;
}

