using ExaminationSystem.Application.Common.Models;

namespace ExaminationSystem.Infrastructure.Services.Notification;

public static class OtpEmailFormatter
{
    public static string GetEmailContent(OtpPurpose purpose, int expiryMinutes, string otpCode)
    {
        return purpose switch
        {
            OtpPurpose.Register => FormatRegistrationEmail(expiryMinutes, otpCode),
            OtpPurpose.ResetPassword => FormatPasswordResetEmail(expiryMinutes, otpCode),
            _ => FormatLoginEmail(expiryMinutes, otpCode)
        };
    }

    private static string FormatRegistrationEmail(int expiryMinutes, string otpCode) =>
        $"<p>Welcome! Your verification code is <strong>{otpCode}</strong>. " +
        $"It expires in <strong>{expiryMinutes} minutes</strong>.</p>";

    private static string FormatPasswordResetEmail(int expiryMinutes, string otpCode) =>
        $"<p>You requested a password reset. Your verification code is <strong>{otpCode}</strong>       . " +
        $"It expires in <strong>{expiryMinutes} minutes</strong>. If you didn't request this, ignore this email.</p>";

    private static string FormatLoginEmail(int expiryMinutes, string otpCode) =>
        $"<p>Your verification code is <strong>{otpCode}</strong>. " +
        $"It expires in <strong>{expiryMinutes} minutes</strong>.</p>";

   
    
}