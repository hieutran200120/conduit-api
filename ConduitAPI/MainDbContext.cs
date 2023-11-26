using System.Linq;
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
        public DbSet<Article> Articles { get; set; }
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditInfo()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is IAuditInfo && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                ((IAuditInfo)entry.Entity).LastUpdatedAt = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    ((IAuditInfo)entry.Entity).CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    Entry(((IAuditInfo)entry.Entity)).Property(x => x.CreatedAt).IsModified = false;
                }
            }
        }
    }
}