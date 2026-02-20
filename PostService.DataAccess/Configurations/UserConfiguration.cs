using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.LastName).IsRequired();

            builder.Property(x => x.Email).IsRequired();
        }
    }
}
