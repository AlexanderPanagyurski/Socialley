using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(x => x.FirstName)
                .IsUnicode(true);

            builder
                .Property(x => x.LastName)
                .IsUnicode(true);

            builder
                .Property(x => x.Email)
                .IsUnicode(false);

            builder
                .Property(x => x.PhoneNumber)
                .IsUnicode(false);

            builder
                .Property(x => x.AboutMe)
                .IsUnicode(true);

            builder
                .HasOne(x => x.City)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Country)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Region)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
