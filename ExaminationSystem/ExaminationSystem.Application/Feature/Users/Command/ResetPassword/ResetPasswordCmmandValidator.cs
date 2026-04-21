using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.Users.Command.ResetPassword
{
    public  sealed class ResetPasswordCmmandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCmmandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
                .Equal(x=>x.ConfirmPassword).WithMessage("Passwords do not match.");

        }
    }

}
