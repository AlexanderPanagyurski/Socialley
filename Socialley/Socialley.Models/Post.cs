using Socialley.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class Post
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        [MaxLength(AttributesConstraints.ContentMaxLength)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string ImageUrl { get; set; }

        public int Likes { get; set; }

        [Required, ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<BlockedPost> BlockedPosts { get; set; } = new HashSet<BlockedPost>();

        public virtual ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();

        public virtual ICollection<FavouritePost> FavouritePosts { get; set; } = new HashSet<FavouritePost>();

    }
}
