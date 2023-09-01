using counduitApi.EntityCommon;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace counduitApi.Entities
{
    public class Comment : BaseEntity<int>
    {
        public string CommentContent { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }


    }
    public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(nameof(Comment), MainDbcontext.CommentSchema);
            builder.HasOne(x => x.User).WithMany(x => x.Comments).HasForeignKey(x => x.UserId);
            builder.HasOne(a => a.Article).WithMany(a => a.Comments).HasForeignKey(a => a.ArticleId);
        }
    }
}
