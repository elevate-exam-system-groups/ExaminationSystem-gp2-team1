
using testexamination.Entity;
using testexamination.Repo;

namespace ExaminationSystem.Infrastructure.Repo
{
    public interface IQuizRepository : IGenericRepository<Quize>
    {
        Task<Quize?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<Quize>> GetByDiplomaIdAsync(Guid diplomaId, CancellationToken cancellationToken = default);
    }
}
