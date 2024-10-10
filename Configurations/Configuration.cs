using BoilerPlate.Data.Context;
using BoilerPlate.Data.DAO.Implementations;
using BoilerPlate.Data.DAO.Interface;
using BoilerPlate.Filters;
using BoilerPlate.Services.Implementations;
using BoilerPlate.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Text;
using System.Text.Json;
namespace BoilerPlate.Configurations;

/// <summary>
/// 
/// </summary>
public static class Configuration
{
    /// <summary>
    /// Helps add services to the Webbuilder
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(typeof(GlobalHttpRequestFilter));
        }).AddNewtonsoftJson(); 
        builder.Services.AddHttpClient();

        // Database Context
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        // Dependency Injections
        builder.Services.AddScoped<IUserDetailsDao, UserDetailsImpl>();
        builder.Services.AddScoped<IAuthUsersDao, AuthUsersImpl>();
        builder.Services.AddScoped<IClientAuthDao, ClientAuthImpl>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
    }

    public static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });



            // Add Authorization header for post-login APIs
            options.OperationFilter<PostLoginHeaderOperationFilter>();
            options.OperationFilter<PreLoginHeaderOperationFilter>();
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }

    public static void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
        .WriteTo.Conditional(
            logEvent => logEvent.Level == LogEventLevel.Information,
            wt => wt.File("Utilities/Logs/Api-Responses/log-respons.txt", rollingInterval: RollingInterval.Day)
        )
        .WriteTo.Conditional(
            logEvent => logEvent.Level == LogEventLevel.Error,
            wt => wt.File("Utilities/Logs/Exception-logs/log-exception.txt", rollingInterval: RollingInterval.Day)
        )
        .WriteTo.File("Utilities/Logs/General/log.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console()

        .CreateLogger();

        builder.Services.AddSerilog();

    }

    public static void ConfigureAuthentication(WebApplicationBuilder builder, string jwtSecretKey)
    {
        builder.Services.AddAuthentication(cfg =>
        {
            cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = false;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

}