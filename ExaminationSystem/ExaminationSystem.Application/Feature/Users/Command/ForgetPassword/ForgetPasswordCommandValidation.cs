using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.ForgetPassword
{
    public  class ForgetPasswordCommandValidation : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidation() {
            RuleFor(x => x.Email)
                   .NotEmpty()
                   .EmailAddress()
                   .WithMessage("Email is required.");
        }

    }
}
