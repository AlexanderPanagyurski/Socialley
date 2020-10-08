using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .Property(x => x.Email)
                .IsUnicode(false);

            builder
                .Property(x => x.PhoneNumber)
                .IsUnicode(false);

            builder
                .Property(x => x.UserFullName)
                .IsUnicode(true);

            builder
                .Property(x => x.Content)
                .IsUnicode(true);

            builder
                .HasOne(x => x.ParentComment)
                .WithMany(y => y.CommentReplies)
                .HasForeignKey(x => x.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Post)
                .WithMany(y => y.Comments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
