using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Socialley.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Data.EntityConfigurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder
                .Property(x => x.Name)
                .IsUnicode(true);

            builder
                .HasOne(x => x.Country)
                .WithMany(y => y.Regions)
                .HasForeignKey(x=>x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
