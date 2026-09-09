using DoseRegistry.Api.Data;
using DoseRegistry.Api.Middleware;
using DoseRegistry.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Configuration/base_config.json", optional: false, reloadOnChange: true);

var dbConnection = builder.Configuration.GetSection("Database:Connection");
var connectionString =
    $"Host={dbConnection["Host"]};" +
    $"Port={dbConnection["Port"]};" +
    $"Database={dbConnection["Database"]};" +
    $"Username={dbConnection["Username"]};" +
    $"Password={dbConnection["Password"]}";

builder.Services.AddDbContext<DoseRegistryDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IAnnualSummaryReportService, AnnualSummaryReportService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
