using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.User.Command.UserRegistration
{
    public sealed record UserRegisteredEvent(string Email) : INotification;
    
}
