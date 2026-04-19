using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using MediatR;

namespace ExaminationSystem.Application.Feature.Otp.Commands.ResendOtp;

/// <summary>
/// Resends (regenerates) an OTP for the given identifier.
/// Rate-limited to 3 requests per hour per identifier/IP (same budget as RequestOtp).
/// </summary>
/// <param name="Identifier">The user's email address.</param>
/// <param name="Purpose">The business flow this OTP belongs to.</param>
/// <param name="ClientIp">Caller IP for per-IP rate limiting.</param>
public sealed record ResendOtpCommand(
    string Identifier,
    OtpPurpose Purpose,
    string? ClientIp = null
) : IRequest<Result<string>>;
