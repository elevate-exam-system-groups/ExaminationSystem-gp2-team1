using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Otp.Commands.RequestOtp;
using ExaminationSystem.Application.Feature.Otp.Commands.ResendOtp;
using ExaminationSystem.Application.Feature.User.Command.UserRegistration;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.User.Events
{
    public class SendOtpOnUserRegistered
      : INotificationHandler<UserRegisteredEvent>
    {
        private readonly ISender _sender;

        public async Task Handle(UserRegisteredEvent notification, CancellationToken ct)
        {
            await _sender.Send(
                new RequestOtpCommand(notification.Email , OtpPurpose.Register),
                ct
            );
        }
    }
}
