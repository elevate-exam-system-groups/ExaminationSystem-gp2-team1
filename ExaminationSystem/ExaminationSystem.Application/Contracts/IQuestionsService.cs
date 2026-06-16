using testexamination.Dtos;

namespace testexamination.IService
{
    public interface IQuestionService
    {
        //Task<QuestionResponseDto> AddQuestionAsync(CreateQuestionDto dto);

        //Task<QuestionResponseDto> UpdateQuestionAsync(int id, UpdateQuestionDto dto);

        //Task DeleteQuestionAsync(int id);

        //Task<QuestionResponseDto> GetQuestionByIdAsync(int id);

        //Task<List<QuestionResponseDto>> GetQuestionsByQuizIdAsync(int quizId);
        Task<QuestionResponseDto> CreateAsync(CreateQuestionDto dto);
        Task<QuestionResponseDto> UpdateAsync(Guid id, CreateQuestionDto dto);
        Task DeleteAsync(Guid id);

        Task<QuestionResponseDto> GetByIdAsync(Guid id);
        Task<List<QuestionResponseDto>> GetByQuizIdAsync(Guid quizId);
    }
}
