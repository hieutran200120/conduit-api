using ConduitAPI.Services.Articles;
using Microsoft.EntityFrameworkCore;

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
        }

        public static void ConfigureMigration(this IServiceCollection services)
        {
            var mainDbContext = services.BuildServiceProvider().GetRequiredService<MainDbContext>();
            if(mainDbContext is not null)
            {
                mainDbContext.Database.Migrate();
            }
        }
    }
}
