using ExaminationSystem.Api.Controllers;
using ExaminationSystem.Application.Feature.User.Command.AccountVerification;
using ExaminationSystem.Application.Feature.User.Command.UserRegistration;
using ExaminationSystem.Contracts.Requests.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    public class AuthController(ISender mediator) : ApiController
    {
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
                         action: nameof(AccountVerification),
                         controller: "Auth",
                         values: new { id = response.Id });

                    return Created(verificationUri, new
                    {
                        Message = "User registered successfully. Please verify your account.",
                        UserId = response.Id,
                        NextStepUrl = verificationUri
                    });
                },
                Problem);
        }

        [HttpPost("verify-otp/{id:guid}")]
        public async Task<IActionResult> AccountVerification([FromRoute] Guid id, [FromBody] int otp)
        {
            var command = new AccountVerificationCommand(id, otp);
            var result = await mediator.Send(command);
            return Ok();
/*
            return result.Match<IActionResult>(
                response => Ok(new { Message = "Account verified successfully" }),
                Problem);*/
        }
    }
}