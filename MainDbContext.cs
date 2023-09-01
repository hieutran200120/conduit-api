using Microsoft.EntityFrameworkCore;

namespace counduitApi
{
    public class MainDbcontext : DbContext
    {
        public static string UserSchema = "user";
        public static string ArticleSchema = "article";
        public static string CommentSchema = "comment";
        public static string Favoriteschema = "Favorite";

        public MainDbcontext(DbContextOptions<MainDbcontext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbcontext).Assembly);
        }
    }

}
