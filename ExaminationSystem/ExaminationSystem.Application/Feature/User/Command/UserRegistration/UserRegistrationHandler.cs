using ExaminationSystem.Domin.Common.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.User.Command.UserRegistration
{
    public class UserRegistrationHandler : IRequestHandler<UserRegistrationCommand, IResult>
    {
        public Task<IResult> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
