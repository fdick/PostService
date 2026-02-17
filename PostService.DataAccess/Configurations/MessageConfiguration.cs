using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.Core.Models;
using PostService.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostService.DataAccess.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Message)
                .HasMaxLength(Message.MAX_MESSAGE_LENGTH)
                .IsRequired();

            builder.Property(x => x.CreateTime).IsRequired();
        }
    }
}
