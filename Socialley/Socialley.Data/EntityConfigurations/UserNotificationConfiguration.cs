using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data.EntityConfigurations
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder
                .Property(x => x.TargetUserFirstName)
                .IsUnicode(true);

            builder
                .Property(x => x.TargetUserLastName)
                .IsUnicode(true);

            builder
                .Property(x => x.Text)
                .IsUnicode(true);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.UserNotifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
