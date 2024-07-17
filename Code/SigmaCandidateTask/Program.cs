using Microsoft.EntityFrameworkCore;
using SigmaCandidateTask.Application.Services;
using SigmaCandidateTask.Core.AutoMapper;
using SigmaCandidateTask.Core.IRepositories;
using SigmaCandidateTask.Core.IServices;
using SigmaCandidateTask.DataAccess.Contexts;
using SigmaCandidateTask.DataAccess.Repositories;
using AutoMapper;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICandidateRepositoryAsync, CandidateRepositoryAsync>();
builder.Services.AddScoped<ICandidateServices, CandidateServices>();
builder.Services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();


builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register Serilog's ILogger for dependency injection
builder.Services.AddSingleton(Log.Logger);


var app = builder.Build();

// Register the Exception Handling Middleware
 app.UseMiddleware<ExceptionHandlingMiddleware>();


// Apply any pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
