using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data.EntityConfigurations
{
    public class RecommenedFriendConfiguration : IEntityTypeConfiguration<RecommendedFriend>
    {
        public void Configure(EntityTypeBuilder<RecommendedFriend> builder)
        {
            builder
                .Property(x => x.RecommendedFirstName)
                .IsUnicode(true);

            builder
                .Property(x => x.RecommendedLastName)
                .IsUnicode(true);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.RecommendedFriends)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
