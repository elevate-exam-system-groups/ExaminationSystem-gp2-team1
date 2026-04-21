using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.ResetPassword;

    public sealed record ResetPasswordCommand(string Email,
        string Token,
        string NewPassword,
        string ConfirmPassword) : IRequest<Result<string>>;
   

