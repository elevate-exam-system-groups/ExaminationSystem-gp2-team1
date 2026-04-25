using System;
using System.Collections.Generic;
using System.Text;
using ExaminationSystem.Application.Common.Models;

namespace ExaminationSystem.Application.Common.Inrastracture
{
    public interface IResetTokenService
    {
        ResetTokenResult GenerateToken();
        string HashToken(string token);
        bool VerifyToken(string token, string hashedToken);

    }
}
