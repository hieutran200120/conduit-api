using counduitApi.EntityCommon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace counduitApi.Entities
{
    public class Article : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContenArticle { get; set; }
        public string slug { get; set; }
        public string[] tags { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Favorite> Facorites { get; set; }

    }
    public class ArticleEntityConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable(nameof(Article), MainDbcontext.ArticleSchema);
            builder.HasOne(a => a.User).WithMany(a => a.Articles).HasForeignKey(a => a.UserId);
        }
    }
}
