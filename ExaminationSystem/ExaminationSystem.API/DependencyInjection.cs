using ExaminationSystem.Infrastructure._Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExaminationSystem.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
        public static IServiceCollection AddAuthentictaion(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            services.AddAuthentication(
                opt => opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme
                )
            .AddJwtBearer(
            opt =>
            {
                var jwtsettings = configuration.GetSection("Jwt").Get<JwtSettings>();
                var key = Encoding.ASCII.GetBytes(jwtsettings.Key);
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {

                    ValidIssuer = jwtsettings.Issuer,
                    ValidAudience = jwtsettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };

            });

        }

    }
}
