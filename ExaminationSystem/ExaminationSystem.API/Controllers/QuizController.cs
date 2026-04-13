using ExaminationSystem.Application.Feature.Quizzes.Command.StartQuiz;
using ExaminationSystem.Domin.Comman.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{quizId}/start")]
        public async Task<IActionResult> StartQuiz(Guid quizId)
        {

            Guid userId = default; // Get the user ID from the authenticated user context (e.g., JWT token, session, etc.)

            var result = await _mediator.Send(
                new StartQuizCommand(quizId, userId));

            if (!result.IsSucess)
            {
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(result.Message),
                    ErrorCode.LimitReached => StatusCode(403, result.Message),
                    ErrorCode.ExistingAttempt => Conflict(result.Message),
                    _ => BadRequest(result.Message)
                };
            }

            return Ok(result.Data);
        }

    }
}
