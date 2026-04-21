using ExaminationSystem.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExaminationSystem.Contracts.Requests.UserUserRequests
{
    public sealed record UserRequests
    {
        public sealed record UserRegistrationRequest(
                    string FirstName,
                    string LastName,
                    string Email,
                    string Password);

        public sealed record RequestOtpRequest(string Identifier, OtpPurpose Purpose);

        public sealed record ResendOtpRequest(string Identifier, OtpPurpose Purpose);

        public sealed record VerifyOtpRequest(string Identifier, OtpPurpose Purpose, string OtpCode);
        public sealed record LoginRequest(string Email, string Password);
        public sealed record ForgetPasswordRequest(string Email);
        public sealed record ResetPasswordRequest(string NewPassword , string ConfirmPassword);

    }
}
