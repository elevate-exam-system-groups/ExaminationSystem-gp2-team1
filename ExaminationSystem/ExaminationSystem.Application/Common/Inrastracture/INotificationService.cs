using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Otp.Commands.RequestOtp;
using ExaminationSystem.Domin.Common.Result;
using Microsoft.Extensions.Logging;

namespace ExaminationSystem.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task<Result<string>> SendOtpEmailAsync(string identifier, OtpPurpose purpose, int expirationMinutes, string plaintextOtp, ILogger logger, CancellationToken cancellationToken);
    Task <Result<String>> SendForgetPasswordEmailAsync(string email, Link token);
}

