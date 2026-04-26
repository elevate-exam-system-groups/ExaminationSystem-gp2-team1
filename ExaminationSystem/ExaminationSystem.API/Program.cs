using ExaminationSystem.API;
using ExaminationSystem.API.Middleware;
using ExaminationSystem.Application;
using ExaminationSystem.Domin.Contracts;
using ExaminationSystem.Infrastructure;
using ExaminationSystem.Infrastructure._UnitOfWork;
using ExaminationSystem.Infrastructure.Repo;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddPresentation(builder.Configuration)
                .AddApplication(builder.Configuration)
                .AddInfrastructure(builder.Configuration);



builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
});


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Always enable Swagger (restrict to Development if preferred)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Examination System API v1");
    options.RoutePrefix = "swagger"; // UI at /swagger
});
app.UseCoreMiddlewares();

app.MapControllers();

app.Run();
