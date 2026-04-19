using Application.common.Models;
using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Infrastructure._Data.Context;
using ExaminationSystem.Infrastructure._UnitOfWork;
using ExaminationSystem.Infrastructure.Identity;
using ExaminationSystem.Infrastructure.Repo;
using ExaminationSystem.Infrastructure.Services.Notification;
using ExaminationSystem.Infrastructure.Services.Otp;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            #region Context
            services.AddDbContext<AppDbContext>((optionsBuilder) =>
            {
                optionsBuilder
                //.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            #region Redis & OTP
            // IConnectionMultiplexer — singleton (thread-safe, expensive to create)
            var redisConnection = configuration["Redis:ConnectionString"] ?? "localhost:6379";
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));

            // IDistributedCache — used by OtpService for get/set/remove
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "ExamSystem:"; // key prefix in Redis
            });

            services.Configure<OtpSettings>(configuration.GetSection("OtpSettings"));
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IRateLimiterService, RateLimiterService>();
            #endregion

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            
            services.AddScoped<INotificationService, NotificationService>();    
            services.AddScoped<IUserService, UserService>();    
            

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;



       

        }
    }

}
