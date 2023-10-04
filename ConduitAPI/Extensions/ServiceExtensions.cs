using System.Text;
using ConduitAPI.Infrastructure.Auth;
using ConduitAPI.Services.Articles;
using ConduitAPI.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ConduitAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<MainDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ConduitAPI")).EnableSensitiveDataLogging());
        }


        public static void ConfigDIBusinessService(this IServiceCollection services)
        {
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void ConfigureMigration(this IServiceCollection services)
        {
            var mainDbContext = services.BuildServiceProvider().GetRequiredService<MainDbContext>();
            if(mainDbContext is not null)
            {
                mainDbContext.Database.Migrate();
            }
        }

        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            var credential = configuration["AppCredential"];
            var key = Encoding.ASCII.GetBytes(credential);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, //người cấp phát
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    //ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
