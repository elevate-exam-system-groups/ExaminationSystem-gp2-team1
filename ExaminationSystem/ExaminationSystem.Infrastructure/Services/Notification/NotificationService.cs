using Application.common.Models;
using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Otp.Commands;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Infrastructure.Services.Notification;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Runtime;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
namespace Infrastructure.Services

{
    public class NotificationService(IOptions<SmtpSettings> smtpSettings) : INotificationService
    {
        private readonly SmtpSettings _smtpSettings = smtpSettings.Value; 

        public async Task SendEmailAsync(
         string to,
         string subject,
         string body, CancellationToken ct)
        {
            //why not static here 
            // Because we might want to inject different SMTP settings for different environments (e.g., development, staging, production) or even for different email providers. By using instance methods and dependency injection,
            // we can easily swap out the SMTP settings without changing the underlying code that sends the emails.
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpSettings.Email));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect(_smtpSettings.SmtpServer, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.Email, _smtpSettings.Password);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true);

        }
        public async Task<Result<string>> SendOtpEmailAsync(
        string identifier,
        OtpPurpose purpose,
        int expirationMinutes,
        string otpCode,
        ILogger _logger,
        CancellationToken cancellationToken = default) 
        {
            var subject = $"Your new {purpose} verification code";
            var body = OtpEmailFormatter.GetOtpEmailContent(purpose, expirationMinutes, otpCode);

            try
            {
                await SendEmailAsync(identifier, subject, body, cancellationToken);
                _logger.LogInformation("OTP dispatched for identifier {Identifier} / purpose {Purpose}",
                    identifier, purpose);

                return new Result<string>(otpCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send OTP notification to {Identifier}", identifier);

                return new Result<string>(new Error(
                    ErrorCode.NotificationFailed,
                    "Failed to send the verification email. Please try again later."
                ));
            }
        }

        public async Task<Result<string>> SendForgetPasswordEmailAsync(string email, Link link)
        {
            var subject = "Password Reset Request";
           
            var baseUrl = link.VerificationUri; 
            var body = ForgetPasswordEmailFormatter.GetForgetPasswordEmailContent(link);

            try
            {
                await SendEmailAsync(email, subject, body, CancellationToken.None);
                return new Result<string>("Password reset email sent successfully.");
            }
            catch (Exception ex)
            {
                return new Result<string>(new Error(
                    ErrorCode.NotificationFailed,
                    "Failed to send the password reset email. Please try again later."
                ));
            }
        }
    }
}