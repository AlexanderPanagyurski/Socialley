using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data.EntityConfigurations
{
    public class BlockedPostConfiguration : IEntityTypeConfiguration<BlockedPost>
    {
        public void Configure(EntityTypeBuilder<BlockedPost> builder)
        {
            builder.HasKey(x => new { x.PostId, x.UserId });

            builder
                .HasOne(x => x.Post)
                .WithMany(y => y.BlockedPosts)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.BlockedPosts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
