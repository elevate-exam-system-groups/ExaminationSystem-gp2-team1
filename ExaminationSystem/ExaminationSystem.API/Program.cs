using ExaminationSystem.API;
using ExaminationSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddPresentation(builder.Configuration)
                .AddApplication()
                .AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IDiplomaQuizService, DiplomaQuizService>();
builder.Services.AddAutoMapper(typeof(DiplomaQuizMappingProfile));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.Run();
