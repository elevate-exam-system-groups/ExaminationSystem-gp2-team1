using ExaminationSystem.API;
using ExaminationSystem.Application;
using ExaminationSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddPresentation(builder.Configuration)
                .AddApplication(builder.Configuration)
                .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
/*app.UseMiddleware<ExceptionHandlingMiddleware>();*//**/
app.Run();
