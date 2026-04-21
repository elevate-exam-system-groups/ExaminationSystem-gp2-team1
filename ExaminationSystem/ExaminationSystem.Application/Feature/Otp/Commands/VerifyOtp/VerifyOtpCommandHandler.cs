using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Domin.Entities.Enums;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserEntity = ExaminationSystem.Entities.User;

namespace ExaminationSystem.Application.Feature.Otp.Commands.VerifyOtp;

public sealed class VerifyOtpCommandHandler(
    IOtpService otpService,
    IRateLimiterService rateLimiter,
    IUnitOfWork unitOfWork,
    IOptions<OtpSettings> otpOptions,
    ILogger<VerifyOtpCommandHandler> logger,
    IGenericRepository<User> _userRepo
) : IRequestHandler<VerifyOtpCommand, Result<bool>>
{
    private readonly OtpSettings _settings = otpOptions.Value;

    public async Task<Result<bool>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var verifyCounterKey = $"ratelimit:verify:{request.Identifier.ToLowerInvariant()}:{request.Purpose}";

        // ── 1. Check if max attempts already exceeded ─────────────────────────
        var attempts = await rateLimiter.GetCounterAsync(verifyCounterKey, cancellationToken);
        if (attempts >= _settings.MaxVerifyAttempts)
        {
            logger.LogWarning(
                "OTP verification blocked — max attempts reached for {Identifier}/{Purpose}",
                request.Identifier, request.Purpose);

            return new Result<bool>(new Error(
                ErrorCode.OtpMaxAttemptsExceeded,
                $"Too many incorrect attempts. Your OTP has been locked. Please request a new one."));
        }

        // ── 2. Verify code via OtpService ─────────────────────────────────────
        var verifyResult = await otpService.VerifyAsync(
            request.Identifier, request.Purpose, request.OtpCode, cancellationToken);

        if (verifyResult.IsError)
        {
            var error = verifyResult.Errors[0];

            // Increment attempt counter only for wrong code (not for expired/not-found)
            if (error.Code == ErrorCode.OtpInvalid)
            {
                var newCount = await rateLimiter.IncrementAsync(verifyCounterKey, cancellationToken);

                logger.LogWarning(
                    "Incorrect OTP for {Identifier}/{Purpose}. Attempt {Count}/{Max}",
                    request.Identifier, request.Purpose, newCount, _settings.MaxVerifyAttempts);

                var remaining = _settings.MaxVerifyAttempts - (int)newCount;
                return new Result<bool>(new Error(
                    ErrorCode.OtpInvalid,
                    remaining > 0
                        ? $"Invalid OTP. {remaining} attempt(s) remaining."
                        : "Invalid OTP. You have been locked out. Please request a new OTP."));
            }

            logger.LogWarning(
                "OTP verification failed for {Identifier}/{Purpose}: {Error}",
                request.Identifier, request.Purpose, error.Description);

            return verifyResult;
        }

        // ── 3. Success — activate user account (Register flow) ────────────────
        // i want to mave this logic to a domain event handler but for now i will keep it here for simplicity
        if (request.Purpose == OtpPurpose.Register)
        {
            var userRepo = unitOfWork.GetRepository<UserEntity>();
            var user = await userRepo
                .Find(u => u.Email == request.Identifier)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is not null && user.Status == AccountStatus.pending)
            {
                user.Status = AccountStatus.active;
                user.IsEmailVerified = true;
                _userRepo.Update(user);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation(
                    "User {Identifier} account activated after successful OTP verification.", request.Identifier);
            }
        }
       
        // ── 4. Reset attempt counter on success ───────────────────────────────
        await rateLimiter.ResetAsync(verifyCounterKey, cancellationToken);

        logger.LogInformation(
            "OTP verified successfully for {Identifier}/{Purpose}", request.Identifier, request.Purpose);

        return new Result<bool>(true);
    }
}
