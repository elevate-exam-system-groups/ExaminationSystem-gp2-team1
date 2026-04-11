using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domin.Common;
using ExaminationSystem.Infrastructure._data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExaminationSystem.Infrastructure.Repostories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : BaseAuditableEntity
    {
        private readonly AppDbContext _context;
        DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> GetAll() => _dbSet.Where(x => !x.IsDeleted).AsNoTracking();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => _dbSet.Where(x => !x.IsDeleted).AnyAsync(predicate, cancellationToken);

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void SoftDelete(Guid id)
        {
            var entity = _dbSet.Find(id);
            if (entity is null)
                return;
            entity!.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDeleteRange(IEnumerable<Guid> ids)
        {
            var entities = _dbSet.Where(e => ids.Contains(e.Id)).ToList();
            var currentTime = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.UpdatedAt = currentTime;
            }
        }

    }
}
