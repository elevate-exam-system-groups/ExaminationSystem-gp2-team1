using testexamination.Dtos;

namespace testexamination.Iservices
{
    public interface IDiplomaQuizService
    {
        Task<IEnumerable<DiplomaQuizDto>> GetDiplomaQuizzesAsync(int diplomaId, int studentId);
    }
}
