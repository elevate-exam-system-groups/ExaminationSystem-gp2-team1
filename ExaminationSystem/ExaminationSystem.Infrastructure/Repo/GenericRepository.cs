using ExaminationSystem.Domin.Common;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Infrastructure._Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExaminationSystem.Infrastructure.Repo
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : AuditableEntity
    {
        private readonly AppDbContext _context;
        DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> GetAll() => _dbSet.Where(x => !x.IsDeleted).AsNoTracking();
        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => _dbSet.Where(x => !x.IsDeleted).AnyAsync(predicate);
        public IQueryable<T> Find(Expression<Func<T, bool>> creiteria)
        {
            return _dbSet.Where(m => !m.IsDeleted).Where(creiteria).AsNoTracking();
        }
        public void Add(T entity)
        {
           _dbSet.Add(entity);
           
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        /// why update async if we are not doing any async operation in it ? 
        /// 
        public async Task<bool> Update(T entity){
            var entry = _dbSet.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            entry.State = EntityState.Modified;
            return true;
        }
        /// <summary>
        /// can't understand this too 
        /// can crash if nullable properties are included without checking for nullability first
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public async Task<bool> UpdateIncludeAsync(T entity, params Expression<Func<T, Object>>[] properties)
        {
           var local = _dbSet.Local.FirstOrDefault(e => e.Id ==  entity.Id);
            EntityEntry<T> entityEntry;
            if (local == null)
            {
                _dbSet.Attach(entity);
                entityEntry = _context.Entry(entity);
            }
            else
            {
                entityEntry = _context.Entry(local);
                entityEntry.CurrentValues.SetValues(entity);
            }
            foreach (var property in properties)
            {
            entityEntry.Property(property).IsModified = true;
            } return true;
        }
        public async Task<bool> SoftDelete(T entity)
        {
           entity.IsDeleted = true;
           entity.DeletedAt = DateTime.UtcNow;
           var isDeleted = await UpdateIncludeAsync(entity, e => e.IsDeleted, e => e.DeletedAt!);
           return isDeleted;
        }
        public async Task<bool> SoftDeleteRange(IEnumerable<T> entities)
        {
            var currentTime = DateTime.UtcNow;
            var isDeleted = false;
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = currentTime;
                isDeleted = await UpdateIncludeAsync(entity, e => e.IsDeleted, e => e.DeletedAt!);
            }
            return isDeleted;
        }


    }

}
