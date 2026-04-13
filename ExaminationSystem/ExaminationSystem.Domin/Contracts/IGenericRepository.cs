using ExaminationSystem.Domin.Comman;
using System.Linq.Expressions;

namespace ExaminationSystem.Domin.Contracts
{
    public interface IGenericRepository<T>
        where T : AuditableEntity
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Find(Expression<Func<T, bool>> creiteria);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        Task<bool> Update(T entity);
        Task<bool> UpdateIncludeAsync(T entity, params Expression<Func<T, Object>>[] properties);
        Task<bool> SoftDelete(T entity);
        Task<bool> SoftDeleteRange(IEnumerable<T> entities);
    }

}


