using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testexamination.Dtos;
using testexamination.IService;

namespace testexamination.Controllers
{
    [ApiController]
    [Route("api/questions")]
    [Authorize(Roles = "Admin")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionDto dto)
            => Ok(await _service.CreateAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateQuestionDto dto)
            => Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("quiz/{quizId}")]
        public async Task<IActionResult> GetByQuiz(Guid quizId)
            => Ok(await _service.GetByQuizIdAsync(quizId));
    }
}
