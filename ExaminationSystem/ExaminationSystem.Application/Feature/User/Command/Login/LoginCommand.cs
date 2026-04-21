using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.Login
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<Result<TokenResponse>>;
    
}
