
using ExaminationSystem.Domin.Common;
using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
namespace ExaminationSystem.Application.Feature.User.Command.AccountVerification
{
    public record AccountVerificationCommand(int otp) : IRequest<Result>;
    
}
