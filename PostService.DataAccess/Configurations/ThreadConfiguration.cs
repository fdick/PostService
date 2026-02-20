using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess.Configurations
{
    public class ThreadConfiguration : IEntityTypeConfiguration<ThreadEntity>
    {
        public void Configure(EntityTypeBuilder<ThreadEntity> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.AuthorID).IsRequired();

            builder.HasMany(x => x.Messages).WithOne(x => x.Thread);

            builder.HasOne(x => x.Author).WithOne();
        }
    }
}
