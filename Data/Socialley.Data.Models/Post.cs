namespace Socialley.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Socialley.Data.Common.Models;

    public class Post : BaseDeletableModel<string>
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<FavoritePost> FavoritePosts { get; set; } = new HashSet<FavoritePost>();

        public virtual ICollection<ImagePost> ImagePosts { get; set; } = new HashSet<ImagePost>();


        public virtual ICollection<PostTag> TagsPosts { get; set; } = new HashSet<PostTag>();

    }
}
