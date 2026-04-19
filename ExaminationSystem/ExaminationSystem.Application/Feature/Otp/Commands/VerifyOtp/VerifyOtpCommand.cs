using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Otp.Commands.VerifyOtp;

/// <summary>
/// Verifies a 6-digit OTP supplied by the user.
/// On success the user's account status is set to "active".
/// On 5 consecutive failures the OTP lock is triggered.
/// </summary>
/// <param name="Identifier">The user's email address.</param>
/// <param name="Purpose">The business flow this OTP was generated for.</param>
/// <param name="OtpCode">The 6-digit plaintext code entered by the user.</param>
public sealed record VerifyOtpCommand(
    string Identifier,
    OtpPurpose Purpose,
    string OtpCode
) : IRequest<Result<bool>>;
