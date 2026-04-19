using ExaminationSystem.Application.Feature.Student.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.API.Controllers
{
    [ApiController]
    [Route("api/student")]

    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
        {
            var studentId = User.FindFirst("uid")?.Value;

            if (studentId is null)
                return Unauthorized();

            var query = new GetStudentDashboardQuery(Guid.Parse(studentId));

            var result = await _mediator.Send(query, cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Errors);
        }
    }
}
