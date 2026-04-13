using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application.Feature.User.Command.UserRegistration
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");
        }

    }
}
