using System.Text;
using BLL.DTO.Identity;
using BLL.Services;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using DAL.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GymWebService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        
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
        
        builder.Services.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<GymWebServiceContext>();
        builder.Services.AddScoped<IUserService, UserService>();
        // Configure IDENTITY
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;
            options.Password.RequiredUniqueChars = 1;
        });
        
        // SEED DATABASE
        var userManager = builder.Services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
        var roleManager = builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<int>>>();
        await ContextSeeder.SeedEssentialsAsync(userManager, roleManager);
        
        //Configuration from AppSettings
        builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
        
        // Adding Authentication - JWT
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
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