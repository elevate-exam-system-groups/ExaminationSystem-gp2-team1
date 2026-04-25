using ExaminationSystem.Application.Common.Inrastracture;
using ExaminationSystem.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ExaminationSystem.Infrastructure.Services.Token
{
    public class ResetTokenService : IResetTokenService
    {


        public ResetTokenResult GenerateToken()
        {
            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            var hashedToken = HashToken(rawToken);

            return new ResetTokenResult(rawToken, hashedToken);
        }

        public string HashToken(string token)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
        }

        public bool VerifyToken(string rawToken, string hashedToken)
        {
            var hashedRawToken = HashToken(rawToken);
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(hashedRawToken),
                Encoding.UTF8.GetBytes(hashedToken));
        }
    }
}
