using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Infrastructure._data.Context;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            #region Context
            services.AddDbContext<AppDbContext>((optionsBuilder) =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });
            #endregion
            services.AddScoped<IUnitOfWork, ExaminationSystem.Infrastructure.UnitOfWork.UnitOfWork>();
            
            
            
            
            return services;

        }
    }
}
