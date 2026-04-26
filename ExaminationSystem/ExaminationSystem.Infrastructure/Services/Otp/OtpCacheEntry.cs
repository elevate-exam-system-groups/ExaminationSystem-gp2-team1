namespace ExaminationSystem.Infrastructure.Services.Otp;

/// <summary>Internal DTO stored in Redis as JSON.</summary>
internal sealed class OtpCacheEntry
{
    public string HashedOtp { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public int AttemptCount { get; set; }
    public bool IsUsed { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
}
