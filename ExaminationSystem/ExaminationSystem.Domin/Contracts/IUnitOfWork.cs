using ExaminationSystem.Domin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Domin.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : AuditableEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
