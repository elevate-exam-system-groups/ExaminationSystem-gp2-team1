namespace ExaminationSystem.Application.Common.Models;

/// <summary>
/// Identifies the business flow that an OTP belongs to.
/// This value forms part of the Redis key: otp:{purpose}:{identifier}
/// </summary>
public enum OtpPurpose
{
    Register,
    Login,
    ResetPassword
}
