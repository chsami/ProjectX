using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services;
using WebApi.Infrastructure.Services.Firebase;

namespace WebApi.Infrastructure
{
    public static class Startup
    {
        public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {

            service.AddDbContext<ProjectDbContext>(options => options.
                UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    b => b.MigrationsAssembly("WebApi")));

            var key = Convert.FromBase64String(configuration.GetSection("TokenKey").Value);

            service.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.GetSection("Firebase:Authority").Value;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetSection("Firebase:ValidIssuer").Value,
                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("Firebase:ValidAudience").Value,
                    ValidateLifetime = true
                };
            });

            service.AddTransient<IFireBaseService, FirebaseService>();

        }
    }
}
