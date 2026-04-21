using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Common.Inrastracture
{
    public interface IResetTokenService
    {
        string GenerateToken();
        string HashToken();
    }
}
