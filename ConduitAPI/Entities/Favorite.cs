using ConduitAPI.EntityCommon;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ConduitAPI.Entities
{
    public class Favorite : BaseEntity<int>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
    public class FavoriteEntityConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable(nameof(Favorite), MainDbContext.ArticleSchema);
            builder.HasOne(x => x.User).WithMany(x => x.Favorites).HasForeignKey(x => x.UserId);
            builder.HasOne(a => a.Article).WithMany(a => a.Favorites).HasForeignKey(a => a.ArticleId);
        }
    }
}
