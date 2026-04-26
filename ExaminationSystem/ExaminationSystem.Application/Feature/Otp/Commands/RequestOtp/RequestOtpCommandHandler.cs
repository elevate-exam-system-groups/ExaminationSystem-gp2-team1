using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Common.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExaminationSystem.Application.Feature.Otp.Commands.RequestOtp;

public sealed class RequestOtpCommandHandler(
    IOtpService otpService,
    INotificationService notificationService,
    IRateLimiterService rateLimiter,
    IOptions<OtpSettings> otpOptions,
    ILogger<RequestOtpCommandHandler> logger
) : IRequestHandler<RequestOtpCommand, Result<string>> 
{
    private readonly OtpSettings _settings = otpOptions.Value;

    public async Task<Result<string>> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        var emailKey = $"ratelimit:send:{request.Identifier.ToLowerInvariant()}";
        var window = TimeSpan.FromMinutes(_settings.SendWindowMinutes);

        var emailAllowed = await rateLimiter.IsAllowedAsync(emailKey, _settings.MaxSendRequestsPerWindow, window, cancellationToken);
        if (!emailAllowed)
        {
            logger.LogWarning("OTP send rate limit exceeded for identifier {Identifier}", request.Identifier);
            return new Result<string>(new Error(ErrorCode.OtpRateLimitExceeded,
                $"Too many OTP requests. Please wait before requesting a new code."));
        }

        if (!string.IsNullOrWhiteSpace(request.ClientIp))
        {
            var ipKey = $"ratelimit:send:ip:{request.ClientIp}";
            var ipAllowed = await rateLimiter.IsAllowedAsync(ipKey, _settings.MaxSendRequestsPerWindow * 3, window, cancellationToken);
            if (!ipAllowed)
            {
                logger.LogWarning("OTP send rate limit exceeded for IP {Ip}", request.ClientIp);
                return new Result<string>(new Error(ErrorCode.OtpRateLimitExceeded,
                    "Too many OTP requests from this location. Please try again later."));
            }
        }

        var generateResult = await otpService.GenerateAsync(request.Identifier, request.Purpose, cancellationToken);
        if (generateResult.IsError)
            return generateResult;

        var plaintextOtp = generateResult.Value;
        var otpSentResult = await notificationService.SendOtpEmailAsync(request.Identifier, request.Purpose, _settings.ExpirationMinutes, plaintextOtp, logger, cancellationToken);
        if (otpSentResult.IsError)
        {
            return new Result<string>(otpSentResult.Errors);
        }

        return new Result<string>("OTP sent successfully. Please check your email.");
    }

 
}
