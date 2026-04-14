using Application.common.Models;
using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Infrastructure._Data.Context;
using ExaminationSystem.Infrastructure._UnitOfWork;
using ExaminationSystem.Infrastructure.Repo;
using Infrastructure.Services;
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
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            
            services.AddScoped<INotificationService, NotificationService>();    
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;



            return services;

        }
    }

}
