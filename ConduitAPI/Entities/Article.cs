using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ConduitAPI.Entitycommon;

namespace ConduitAPI.Entities
{
    public class Article :BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Conten { get; set; }
        public string Slug { get; set; }
        public string[] Tags { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Favorite> Favorites { get; set; }

    }
    public class ArticleEntityConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable(nameof(Article), MainDbContext.ArticleSchema);
            builder.HasOne(a => a.User).WithMany(a => a.Articles).HasForeignKey(a => a.UserId);
        }
    }
}
