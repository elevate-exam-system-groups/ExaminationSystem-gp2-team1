using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using testexamination.Dtos;
using testexamination.Iservices;

namespace testexamination.Controller
{
    [ApiController]
    [Route("api/diplomas")]
    [Authorize] 
    public class DiplomasController : ControllerBase
    {
        private readonly IDiplomaQuizService _diplomaQuizService;

        public DiplomasController(IDiplomaQuizService diplomaQuizService)
        {
            _diplomaQuizService = diplomaQuizService;
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
    }
}
