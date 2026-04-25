using FluentValidation;

namespace ExaminationSystem.Application.Feature.Otp.Commands.VerifyOtp;

public sealed class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(255).WithMessage("Email address must not exceed 255 characters.");

        RuleFor(x => x.OtpCode)
            .NotEmpty().WithMessage("OTP code is required.")
            .Length(6).WithMessage("OTP code must be exactly 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("OTP code must contain only digits.");

        RuleFor(x => x.Purpose)
            .IsInEnum().WithMessage("OTP purpose must be Register, Login, or ResetPassword.");
    }
}
