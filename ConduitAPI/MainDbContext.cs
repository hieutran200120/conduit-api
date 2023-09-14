using ConduitAPI.Entities;
using ConduitAPI.EntityCommon;
using Microsoft.EntityFrameworkCore;

namespace ConduitAPI
{
    public class MainDbContext : DbContext
    {
        public static string UserSchema = "user";
        public static string ArticleSchema = "article";
        public DbSet<User> Users { get; set; }
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }

    }
}
