using testexamination.Dtos;
using testexamination.Entity;
using testexamination.IService;
using testexamination.Repo;

namespace testexamination.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepo;
        private readonly IQuizRepository _quizRepo;

        public QuestionService(IQuestionRepository questionRepo, IQuizRepository quizRepo)
        {
            _questionRepo = questionRepo;
            _quizRepo = quizRepo;
        }

        public async Task<QuestionResponseDto> CreateAsync(CreateQuestionDto dto)
        {
            var quiz = await _quizRepo.GetByIdAsync(dto.QuizId);

            if (quiz == null)
                throw new Exception("Quiz not found");

            if (quiz.IsPublished)
                throw new Exception("Quiz already published");

            if (dto.Options.Count < 2)
                throw new Exception("Min 2 options required");

            if (dto.Options.Count(o => o.IsCorrect) != 1)
                throw new Exception("Exactly one correct option required");

            var question = new Question
            {
                QuizId = dto.QuizId,
                Text = dto.Text,
                OrderIndex = dto.OrderIndex,
                Explanation = dto.Explanation,
                Options = dto.Options.Select(o => new QuestionOption
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect,
                    OrderIndex = o.OrderIndex
                }).ToList()
            };

            await _questionRepo.AddAsync(question);

            return Map(question);
        }

        public async Task<QuestionResponseDto> UpdateAsync(Guid id, CreateQuestionDto dto)
        {
            var question = await _questionRepo.GetByIdWithOptionsAsync(id);

            if (question == null)
                throw new Exception("Question not found");

            if (question.Quiz.IsPublished)
                throw new Exception("Quiz published");

            question.Text = dto.Text;
            question.OrderIndex = dto.OrderIndex;
            question.Explanation = dto.Explanation;

            question.Options.Clear();

            question.Options = dto.Options.Select(o => new QuestionOption
            {
                Text = o.Text,
                IsCorrect = o.IsCorrect,
                OrderIndex = o.OrderIndex
            }).ToList();

            await _questionRepo.UpdateAsync(question);

            return Map(question);
        }

        public async Task DeleteAsync(Guid id)
        {
            var question = await _questionRepo.GetByIdAsync(id);

            if (question == null)
                throw new Exception("Not found");

            if (question.Quiz.IsPublished)
                throw new Exception("Quiz published");

            await _questionRepo.DeleteAsync(question);
        }

        public async Task<QuestionResponseDto> GetByIdAsync(Guid id)
        {
            var question = await _questionRepo.GetByIdWithOptionsAsync(id);
            return Map(question);
        }

        public async Task<List<QuestionResponseDto>> GetByQuizIdAsync(Guid quizId)
        {
            var questions = await _questionRepo.GetByQuizIdAsync(quizId);
            return questions.Select(Map).ToList();
        }

        private QuestionResponseDto Map(Question q)
        {
            return new QuestionResponseDto
            {
                Id = q.Id,
                Text = q.Text,
                OrderIndex = q.OrderIndex,
                Explanation = q.Explanation,
                Options = q.Options.Select(o => new QuestionOptionResponseDto
                {
                    Id = o.Id,
                    Text = o.Text,
                    OrderIndex = o.OrderIndex
                }).ToList()
            };
        }
    }
}
