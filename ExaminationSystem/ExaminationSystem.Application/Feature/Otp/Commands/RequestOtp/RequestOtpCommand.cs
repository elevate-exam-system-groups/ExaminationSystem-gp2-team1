using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Otp.Commands.RequestOtp;

/// <summary>
/// Generates a new OTP and sends it to the user's email.
/// This command is only valid for unauthenticated users
/// (registration flow, first-time login, etc.).
/// </summary>
/// <param name="Identifier">The user's email address.</param>
/// <param name="Purpose">The business flow this OTP belongs to.</param>
/// <param name="ClientIp">Caller IP used for per-IP rate limiting.</param>
public sealed record RequestOtpCommand(
    string Identifier,
    OtpPurpose Purpose,
    string? ClientIp = null
) : IRequest<Result<string>>;
