using FluentValidation;

namespace ExaminationSystem.Application.Feature.Otp.Commands.ResendOtp;

public sealed class ResendOtpCommandValidator : AbstractValidator<ResendOtpCommand>
{
    public ResendOtpCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(255).WithMessage("Email address must not exceed 255 characters.");

        RuleFor(x => x.Purpose)
            .IsInEnum().WithMessage("OTP purpose must be Register, Login, or ResetPassword.");
    }
}
