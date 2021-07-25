namespace Socialley.Data.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Socialley.Data.Models;

    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> postTag)
        {
            postTag.HasKey(x => new { x.PostId, x.TagId });

            postTag
               .HasOne(x => x.Post)
               .WithMany(y => y.TagsPosts)
               .HasForeignKey(x => x.PostId)
               .OnDelete(DeleteBehavior.Restrict);

            postTag
                .HasOne(x => x.Tag)
                .WithMany(y => y.TagsPosts)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
