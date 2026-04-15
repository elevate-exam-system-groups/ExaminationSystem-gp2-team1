using ExaminationSystem.Api.Controllers;
using ExaminationSystem.Application.Feature.User.Command.AccountVerification;
using ExaminationSystem.Application.Feature.User.Command.UserRegistration;
using ExaminationSystem.Contracts.Requests.User;
using ExaminationSystem.Domin.Common.Result;
using ExaminationSystem.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    public class AuthController(ISender mediator) :ApiController
    {
        private readonly ISender _mediator;
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationRequest request,LinkGenerator linkGenerator , CancellationToken ct)
        {
            var comman = new UserRegistrationCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);
            var result = await mediator.Send(comman , ct);

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



        [HttpPost]
        [Route("/verify-otp/{Guid:id}")]
        public async Task<IActionResult> AccountVerification([FromQuery]Guid id ,[FromBody]int otp)
        {
            var command = new AccountVerificationCommand(id,otp);
            var result = await _mediator.Send(otp);
            return Ok();
        }


    }
}
