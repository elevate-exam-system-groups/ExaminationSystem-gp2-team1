using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Dtos;
using ExaminationSystem.Application.Feature.Diplomas.Command;
using ExaminationSystem.Application.Feature.Diplomas.Query;
using ExaminationSystem.Application.ViewModels.Diplomas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace testexamination.Controller
{
    [ApiController]
    [Route("api/diplomas")]
    [Authorize]
    public class DiplomasController : ControllerBase
    {
        private readonly IDiplomaQuizService _diplomaQuizService;
        private readonly IMediator _mediator;

        public DiplomasController(IDiplomaQuizService diplomaQuizService, IMediator mediator)
        {
            _diplomaQuizService = diplomaQuizService;
            _mediator = mediator;
        }


        [HttpGet("{id}/quizzes")]
        [ProducesResponseType(typeof(IEnumerable<DiplomaQuizDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDiplomaQuizzes([FromRoute] int id)
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (studentIdClaim is null || !int.TryParse(studentIdClaim, out int studentId))
                return Unauthorized();

            var quizzes = await _diplomaQuizService.GetDiplomaQuizzesAsync(id, studentId);
            return Ok(quizzes);
        }
        [HttpGet("GetAllDiplomas")]
        public async Task<IActionResult> GetAllDiplomas()
        {
            var diplomasResult = await _mediator.Send(new GetAllDiplomasQuery());
            return Ok(diplomasResult);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiplomaById([FromRoute] Guid id)
        {
            var diplomaResult = await _mediator.Send(new GetDiplomaByIdQuery(id));
            return Ok(diplomaResult);
        }
        [HttpPost("AddDiploma")]
        public async Task<IActionResult> AddDiploma([FromBody] AddDiplomaViewModel request)
        {
            var addDiplomaResult = await _mediator.Send(new AddDiplomaCommand(request.Title, request.Description));
            return Ok(addDiplomaResult);
        }
        [HttpPut("UpdateDiploma")]
        public async Task<IActionResult> UpdateDiploma([FromBody] UpdateDiplomaViewModel request)
        {
            var updateDiplomaResult = await _mediator.Send(new UpdateDiplomaCommand(request.Id, request.Title, request.Description));
            return Ok(updateDiplomaResult);
        }
    }
}