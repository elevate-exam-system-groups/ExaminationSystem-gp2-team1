using ExaminationSystem.Api.Controllers;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Otp.Commands.RequestOtp;
using ExaminationSystem.Application.Feature.Otp.Commands.ResendOtp;
using ExaminationSystem.Application.Feature.Otp.Commands.VerifyOtp;
using ExaminationSystem.Application.Feature.User.Command.AccountVerification;
using ExaminationSystem.Application.Feature.User.Command.UserRegistration;
using ExaminationSystem.Application.Feature.User.Commond.UserRegistration;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using static ExaminationSystem.Contracts.Requests.UserUserRequests.UserRequests;

namespace ExaminationSystem.API.Controllers
{
    public class AuthController(ISender mediator , IHttpContextAccessor httpContextAccessor) : ApiController
    {
        private IHttpContextAccessor httpContextAccessor;

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationRequest request, [FromServices] LinkGenerator linkGenerator, CancellationToken ct)
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

   /*     [HttpPost("verify-otp/{id:guid}")]
        public async Task<IActionResult> AccountVerification([FromRoute] Guid id, [FromBody] int otp)
        {
            var command = new AccountVerificationCommand(id, otp);
            var result = await mediator.Send(command);
            return Ok();
            *//*
                        return result.Match<IActionResult>(
                            response => Ok(new { Message = "Account verified successfully" }),
                            Problem);*//*
        }*/

      /*  [HttpPost("request-otp")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> RequestOtp(
            [FromBody] RequestOtpRequest request,
            CancellationToken ct)
        {
            var clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var command = new RequestOtpCommand(request.Identifier, request.Purpose, clientIp);
            var result = await mediator.Send(command, ct);

            return result.Match<IActionResult>(
                message => Ok(new { Success = true, Message = message }),
                Problem);
        }*/

     
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
    }

}
