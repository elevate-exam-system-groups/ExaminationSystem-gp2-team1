using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.User.Events
{
   public record UserRegisteredEvent(Guid UserId, string Email) : INotification;
}
