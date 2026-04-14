using Application.common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services , IConfiguration config)
        {
            services.Configure<SmtpSettings>(config.GetSection("EmailSettings"));
        }
    }
}
