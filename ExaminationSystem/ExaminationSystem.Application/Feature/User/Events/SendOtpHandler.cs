using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.Extensions.Logging; // Added for logging
using System.Security.Cryptography;

namespace ExaminationSystem.Application.Feature.User.Events
{
    public class SendOtpHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IGenericRepository<OtpRecord> _otpRepo;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SendOtpHandler> _logger; // Logger field

        public SendOtpHandler(
            IGenericRepository<OtpRecord> otpRepo,
            INotificationService notificationService,
            IUnitOfWork unitOfWork,
            ILogger<SendOtpHandler> logger)
        {
            _otpRepo = otpRepo;
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting OTP generation for User: {UserId}", notification.UserId);

           
                var otpCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

                var otpRecord = new OtpRecord
                {
                    UserId = notification.UserId,
                    OtpHash = BCrypt.Net.BCrypt.HashPassword(otpCode),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                    IsUsed = false
                };

                _otpRepo.Add(otpRecord);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
        {
                    _logger.LogError("Failed to save OTP record for User: {UserId}", notification.UserId);
                    return;
                }   
                _logger.LogInformation("OTP record saved successfully for User: {UserId}", notification.UserId);

            var emailBody = $@"
                <div style='font-family: sans-serif;'>
                    <h2>Hello {notification.FirstName},</h2>
                    <p>Your verification code for the Examination System is:</p>
                    <h1 style='color: #2c3e50;'>{otpCode}</h1>
                    <p>This code is valid for 5 minutes.</p>
                </div>";

            await _notificationService.SendEmailAsync(
                notification.Email,
                "Account Verification - OTP",
                emailBody,
                cancellationToken);

            _logger.LogInformation("OTP email sent successfully to: {Email}", notification.Email);

            throw new NotImplementedException();
        }
    }
}
