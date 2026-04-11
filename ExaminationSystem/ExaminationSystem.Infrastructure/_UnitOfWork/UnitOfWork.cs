using ExaminationSystem.Domin.Comman;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Infrastructure._Data.Context;
using ExaminationSystem.Infrastructure.Repo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Infrastructure._UnitOfWork
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repository = new();

        public void Dispose() => _context.Dispose();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

        public IGenericRepository<T> GetRepository<T>() where T : AuditableEntity
        {
            return (IGenericRepository<T>)_repository.GetOrAdd(typeof(T).FullName!, new GenericRepository<T>(_context));
        }
    }

}
