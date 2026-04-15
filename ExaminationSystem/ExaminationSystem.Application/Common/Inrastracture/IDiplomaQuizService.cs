using ExaminationSystem.Application.Dtos;
namespace ExaminationSystem.Application.Common.Interfaces;

    public interface IDiplomaQuizService
    {
        Task<IEnumerable<DiplomaQuizDto>> GetDiplomaQuizzesAsync(int diplomaId, int studentId);
    }

