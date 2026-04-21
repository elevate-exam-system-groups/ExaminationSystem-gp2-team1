using ExaminationSystem.Application.Common.Models;

namespace ExaminationSystem.Infrastructure.Services.Notification;

public static class ForgetPasswordEmailFormatter
{
    /// <summary>
    /// Generates the password reset email content with a reset link.
    /// </summary>
    /// <param name="token">The reset token to include in the link</param>
    /// <param name="email">The user's email address</param>
    /// <param name="baseUrl">The base URL of the application (e.g., https://yourdomain.com)</param>
    /// <param name="expirationHours">The token expiration time in hours</param>
    /// <returns>HTML formatted email content</returns>
    public static string GetForgetPasswordEmailContent(
      Link link, 
        int expirationHours = 1)
    {
        var resetLink = $"{link.VerificationUri}/auth/reset-password?token={Uri.EscapeDataString(link.Token)}&email={Uri.EscapeDataString(link.Email)}";

        return $@"
<html>
  <body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333;"">
    <div style=""max-width: 600px; margin: 0 auto; padding: 20px;"">
      <h2>Password Reset Request</h2>
      <p>We received a request to reset your password. Click the link below to proceed:</p>
      <p>
        <a href=""{resetLink}"" style=""display: inline-block; padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;"">
          Reset Your Password
        </a>
      </p>
      <p style=""font-size: 14px; color: #666;"">
        Or copy and paste this link in your browser:<br/>
        <code>{resetLink}</code>
      </p>
      <p>This link will expire in <strong>{expirationHours} hour(s)</strong>.</p>
      <p style=""color: #666; font-size: 12px;"">
        If you didn't request a password reset, you can safely ignore this email. Your account remains secure.
      </p>
    </div>
  </body>
</html>";
    }
}

