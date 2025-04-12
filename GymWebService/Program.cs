using BLL.Services;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using DAL.UOW;
using Microsoft.EntityFrameworkCore;

namespace GymWebService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
            builder.Services.AddDbContext<GymWebServiceContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DockerPostgreSQLConnection")));
        }
        else if (builder.Environment.IsProduction())
        {
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Services.AddDbContext<GymWebServiceContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStringPostgres")));
        }
        
        // REPOSITORIES
        builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IWorkoutTemplateRepository, WorkoutTemplateRepository>();
        builder.Services.AddScoped<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // SERVICES
        builder.Services.AddScoped<IExerciseService, ExerciseService>();
        builder.Services.AddScoped<IWorkoutService, WorkoutService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IWorkoutTemplateService, WorkoutTemplateService>();
        builder.Services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();
        
        
        // AUTOMAPPER
        //builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddAutoMapper(typeof(Program));

        var app = builder.Build();

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
    }
}