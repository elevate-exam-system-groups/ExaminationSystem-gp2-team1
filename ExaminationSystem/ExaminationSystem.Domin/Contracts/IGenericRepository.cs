using ExaminationSystem.Domin.Common;
using System.Linq.Expressions;

namespace ExaminationSystem.Domain.Contracts
{
    public interface IGenericRepository<T>
        where T : BaseAuditableEntity
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void SoftDelete(Guid id);
        void SoftDeleteRange(IEnumerable<Guid> ids);
    }
}