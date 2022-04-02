using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using WebApi.Infrastructure.Database;

namespace WebApi.Infrastructure
{
    public static class Startup
    {
        public static void AddInfrastructure(this IServiceCollection service)
        {

            var builder = new NpgsqlConnectionStringBuilder("")
            {
                Password = ""
            };

            service.AddDbContext<ProjectDbContext>(options => options.UseNpgsql(builder.ConnectionString, b => b.MigrationsAssembly("WebApi")));

            var key = Convert.FromBase64String("ErfQ8PB37UywFx9vCunQAw==");

            service.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
