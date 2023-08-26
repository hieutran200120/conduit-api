using Microsoft.EntityFrameworkCore;

namespace ConduitAPI
{
    public class MainDbContext : DbContext
    {
        public static string UserSchema = "user";
        public static string ArticleSchema = "article";

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }
    }
}
