using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExaminationSystem.Application.Feature.Otp.Commands.ResendOtp;

public sealed class ResendOtpCommandHandler(
    IOtpService otpService,
    INotificationService notificationService,
    IRateLimiterService rateLimiter,
    IOptions<OtpSettings> otpOptions,
    ILogger<ResendOtpCommandHandler> logger
) : IRequestHandler<ResendOtpCommand, Result<string>>
{
    private readonly OtpSettings _settings = otpOptions.Value;

    public async Task<Result<string>> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
    {
        // ── 1. Rate-limit: 3 sends per hour per email ────────────────────────
        var emailKey = $"ratelimit:send:{request.Identifier.ToLowerInvariant()}";
        var window = TimeSpan.FromMinutes(_settings.SendWindowMinutes);

        var emailAllowed = await rateLimiter.IsAllowedAsync(
            emailKey, _settings.MaxSendRequestsPerWindow, window, cancellationToken);

        if (!emailAllowed)
        {
            logger.LogWarning("OTP resend rate limit exceeded for {Identifier}", request.Identifier);
            return new Result<string>(new Error(
                ErrorCode.OtpRateLimitExceeded,
                "You have reached the maximum number of OTP requests (3 per hour). Please wait before trying again."));
        }

        if (!string.IsNullOrWhiteSpace(request.ClientIp))
        {
            var ipKey = $"ratelimit:send:ip:{request.ClientIp}";
            var ipAllowed = await rateLimiter.IsAllowedAsync(
                ipKey, _settings.MaxSendRequestsPerWindow * 3, window, cancellationToken);

            if (!ipAllowed)
            {
                logger.LogWarning("OTP resend rate limit exceeded for IP {Ip}", request.ClientIp);
                return new Result<string>(new Error(
                    ErrorCode.OtpRateLimitExceeded,
                    "Too many OTP requests from this location. Please try again later."));
            }
        }

        // ── 2. Invalidate any existing OTP and generate a fresh one ───────────
        await otpService.InvalidateAsync(request.Identifier, request.Purpose, cancellationToken);

        var generateResult = await otpService.GenerateAsync(
            request.Identifier, request.Purpose, cancellationToken);

        if (generateResult.IsError)
            return generateResult;

        var plaintextOtp = generateResult.Value;
        var otpSentResult = await notificationService.SendOtpEmailAsync(request.Identifier, request.Purpose, _settings.ExpirationMinutes, plaintextOtp, logger, cancellationToken);
        if (otpSentResult.IsError)
        {
            return new Result<string>(otpSentResult.Errors);
        }

        return new Result<string>("A new OTP has been sent to your email. Please check your inbox.");
    }
}
