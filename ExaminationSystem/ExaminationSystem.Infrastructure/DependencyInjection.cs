using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Infrastructure._Data.Context;
using ExaminationSystem.Infrastructure._UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
                //.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });
            #endregion
            services.AddScoped<IUnitOfWork,UnitOfWork>();




            return services;

        }
    }

}
