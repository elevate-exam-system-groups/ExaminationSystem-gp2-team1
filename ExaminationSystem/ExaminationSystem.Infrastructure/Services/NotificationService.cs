using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Application.common.Models;
using ExaminationSystem.Application.Common.Interfaces;
namespace Infrastructure.Services

{
    public class NotificationService(SmtpSettings smtpSettings) : INotificationService
    {
        private readonly SmtpSettings _smtpSettings;
     
        public async Task SendEmailAsync(
         string to,
         string subject,
         string body , CancellationToken ct)
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
    }
}
