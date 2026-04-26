using Application.common.Models;
using ExaminationSystem.Application.Common.Behaviours;
using ExaminationSystem.Application.Common.Interfaces;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Users.Mapping;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ExaminationSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("EmailSettings"));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddScoped<IDiplomaQuizService, DiplomaQuizService>();
            //services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));

            });
            return services;
        }
    }
}
