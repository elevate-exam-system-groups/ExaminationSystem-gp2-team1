using ExaminationSystem.Application.Common.Inrastracture;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ExaminationSystem.Infrastructure.Services.Token
{
    public class ResetTokenService : IResetTokenService
    {
        public string GenerateToken() =>
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
           .Replace("+", "-")
           .Replace("/", "_")
           .Replace("=", "");

        public string HashToken()
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(GenerateToken())));
        }
    }
}
