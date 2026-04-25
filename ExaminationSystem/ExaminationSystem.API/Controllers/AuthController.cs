using ExaminationSystem.Api.Controllers;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Otp.Commands.RequestOtp;
using ExaminationSystem.Application.Feature.Otp.Commands.ResendOtp;
using ExaminationSystem.Application.Feature.Otp.Commands.VerifyOtp;
using ExaminationSystem.Application.Feature.Users.Command.AccountVerification;
using ExaminationSystem.Application.Feature.Users.Command.ForgetPassword;
using ExaminationSystem.Application.Feature.Users.Command.Login;
using ExaminationSystem.Application.Feature.Users.Command.ResetPassword;
using ExaminationSystem.Application.Feature.Users.Command.UserRegistration;
using ExaminationSystem.Application.Feature.Users.Commond.UserRegistration;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using static ExaminationSystem.Contracts.Requests.UserUserRequests.UserRequests;

namespace ExaminationSystem.API.Controllers
{
    public class AuthController(ISender mediator, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : ApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationRequest request, CancellationToken ct)
        {
            var command = new UserRegistrationCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            var result = await mediator.Send(command, ct);

            return result.Match<IActionResult>(
                response =>
                {
                    var verificationUri = linkGenerator.GetUriByAction(
                         HttpContext,
                         action: nameof(VerifyOtp),
                         controller: "Auth"
                         );

                    return Created(verificationUri, new
                    {
                        Message = "User registered successfully. Please verify your account.",
                        UserId = response.Id,
                        NextStepUrl = verificationUri
                    });
                },
                Problem);
        }


        [HttpPost("verify-otp")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> VerifyOtp(
            [FromBody] VerifyOtpRequest request,
            CancellationToken ct)
        {
            var command = new VerifyOtpCommand(request.Identifier, request.Purpose, request.OtpCode);
            var result = await mediator.Send(command, ct);

            return result.Match<IActionResult>(
                _ => Ok(new { Success = true, Message = "OTP verified successfully. Your account is now active." }),
                Problem);
        }


        [HttpPost("resend-otp")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ResendOtp(
            [FromBody] ResendOtpRequest request,
            CancellationToken ct)
        {
            var clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var command = new ResendOtpCommand(request.Identifier, request.Purpose, clientIp);
            var result = await mediator.Send(command, ct);

            return result.Match<IActionResult>(
                message => Ok(new { Success = true, Message = message }),
                Problem);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken ct)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await mediator.Send(command, ct);

            return result.Match<IActionResult>(
                tokenResponse => Ok(tokenResponse),
                Problem);
        }

        [HttpPost("forget-password")]
        [EnableRateLimiting("SlidingWindow")]
        public async Task<IActionResult> ForgetPassword(
            [FromBody] ForgetPasswordRequest request,
            CancellationToken ct)
        {
            var command = new ForgetPasswordCommand(request.Email);
            var result = await mediator.Send(command, ct);

            return result.Match<IActionResult>(
                message => Ok(new { Success = true, Message = message }),
                Problem);
        }

        [HttpPost("reset-password/{token}&&{email}")]
        public async Task<IActionResult> ResetPassword(
            [FromQuery] string token,
            [FromQuery] string email,
            [FromBody] ResetPasswordRequest request,
            CancellationToken ct)
        {
            var command = new ResetPasswordCommand(email, token, request.NewPassword, request.ConfirmPassword);
            var result = await mediator.Send(command, ct);
            return result.Match<IActionResult>(
                message => Ok(new { Success = true, Message = message }),
                Problem

                );


        }

    }
}
