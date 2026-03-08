using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.Core.Models;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.HasKey(x => x.ID);

            builder.HasOne(x => x.User).WithMany(x => x.Posts).HasForeignKey(x => x.UserID);  

            builder.Property(x => x.Message)
                .HasMaxLength(Post.MAX_MESSAGE_LENGTH)
                .IsRequired();

            builder.Property(x => x.CreateTime).IsRequired();

            builder.Property(x => x.UserID).IsRequired();
        }
    }
}
