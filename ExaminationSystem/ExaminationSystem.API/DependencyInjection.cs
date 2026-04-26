using ExaminationSystem.API.Middleware;
using ExaminationSystem.Application.Common.Models;
using ExaminationSystem.Application.Feature.Users.Mapping;
using ExaminationSystem.Infrastructure._Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

namespace ExaminationSystem.API
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            // Add services to the container.
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            services.AddSwaggerGen();

            // Swagger / OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Examination System API",
                    Version = "v1",
                    Description = "API for the Examination System"
                });


                // Add JWT Bearer authentication support in Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token. Example: eyJhbGci..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
            services.AddAuthentictaion(configuration)
                .AddAppRateLimiting();
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
            return services;
        }
        public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddSlidingWindowLimiter("SlidingWindow", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.SegmentsPerWindow = 6;
                    limiterOptions.QueueLimit = 10;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.AutoReplenishment = true;
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }




        public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app)
        {
            // 1. Exception handling should be FIRST to catch all errors
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            // 2. Status code pages for handling HTTP status codes
            app.UseStatusCodePages();

            // 3. HTTPS redirection (before any other middleware that might generate URLs)
            app.UseHttpsRedirection();

            /*     // 4. Serilog request logging (early to log all requests)
                 app.UseSerilogRequestLogging();*/



            // 6. Rate limiting (before authentication to protect auth endpoints)
            app.UseRateLimiter();

            // 7. Authentication (must come before authorization)
            app.UseAuthentication();

            // 8. Authorization (must come after authentication)
            app.UseAuthorization();



            return app;
        }
    }
}
