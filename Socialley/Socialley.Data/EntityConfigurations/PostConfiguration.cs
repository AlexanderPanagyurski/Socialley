using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .Property(x => x.Content)
                .IsUnicode(true);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.Posts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
