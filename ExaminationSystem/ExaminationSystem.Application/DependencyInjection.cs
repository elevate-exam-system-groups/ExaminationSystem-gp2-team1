using Application.common.Models;
using ExaminationSystem.Application.Common.Behaviours;
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
        public static void AddApplication(this IServiceCollection services , IConfiguration config)
        {
            services.Configure<SmtpSettings>(config.GetSection("EmailSettings"));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
               
            });
        }
    }
}
