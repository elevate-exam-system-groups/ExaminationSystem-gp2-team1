using ExaminationSystem.Domin.Comman;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExaminationSystem.Domin.Contracts
{
    public interface IGenericRepository1<T> where T : AuditableEntity
    {
        Task<bool> Add(T entity);

        IQueryable<T> GetAll(Expression<Func<T, bool>>? creiteria = null);

        IQueryable<T> GetbyId(Guid Id);
        Task<bool> UpdateIncludeAsync(T entity, params string[] modifiedParams);
        Task<bool> UpdateIncludeAsync(T entity, params Expression<Func<T, object>>[] properties);
        Task<bool> Update(T entity);

        Task<bool> IsExist(Expression<Func<T, bool>> creiteria);

        Task<bool> Delete(Guid Id);
        Task<bool> SoftDeleteAsync(T entity);
        IQueryable<T> Find(Expression<Func<T, bool>> creiteria);
    }
}
