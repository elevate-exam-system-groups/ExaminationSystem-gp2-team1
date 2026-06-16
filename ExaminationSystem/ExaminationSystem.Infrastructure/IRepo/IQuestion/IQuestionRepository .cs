
using testexamination.Entity;
using testexamination.Repo;

namespace ExaminationSystem.Infrastructure.Repo
{
    public interface IQuestionRepository : IGenericRepository<Questions>
    {
        Task<Questions?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<Questions>> GetByQuizIdAsync(Guid quizId, CancellationToken cancellationToken = default);
    }
}
