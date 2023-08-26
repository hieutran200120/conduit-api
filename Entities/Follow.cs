using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ConduitAPI.EntityCommon;

namespace ConduitAPI.Entities
{
    public class Follow : BaseEntity<int>
    {
        public Guid FollowingId { get; set; }
        public User Following { get; set; }
        public Guid FollowerId { get; set; }
        public User Follower { get; set;}
    }

    public class FollowEntityConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.ToTable(nameof(Follow), MainDbContext.UserSchema);
            builder.HasOne(x => x.Following).WithMany(x => x.Followings).HasForeignKey(x => x.FollowingId);
            builder.HasOne(a => a.Follower).WithMany(a => a.Followers).HasForeignKey(a => a.FollowerId);
        }
    }
}