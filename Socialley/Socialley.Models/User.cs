using Socialley.Common;
using Socialley.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        [Required]
        [MaxLength(AttributesConstraints.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.NameMaxLength)]
        public string LastName { get; set; }


        [Required]
        [MaxLength(AttributesConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegidteredOn { get; set; }

        public DateTime? BannedOn { get; set; }

        public GenderType? Gender { get; set; }

        public string ImageUrl { get; set; }

        public string CoverImageUrl { get; set; }

        [MaxLength(AttributesConstraints.AboutMeMaxLength)]
        public string AboutMe { get; set; }

        public bool IsBlocked { get; set; }

        public string TwitterUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string InstagramUrl { get; set; }

        public string LinkedinUrl { get; set; }

        public string GithubUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Country))]
        public string CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Required]
        [ForeignKey(nameof(City))]
        public string CityId { get; set; }
        public virtual City City { get; set; }


        [Required]
        [ForeignKey(nameof(Region))]
        public string RegionId { get; set; }
        public virtual Region Region { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<UserGroup> UsersGroups { get; set; } = new HashSet<UserGroup>();

         public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public virtual ICollection<PostLike> PostsLikes { get; set; } = new HashSet<PostLike>();

        public virtual ICollection<UserNotification> UserNotifications { get; set; } = new HashSet<UserNotification>();

        public virtual ICollection<BlockedPost> BlockedPosts { get; set; } = new HashSet<BlockedPost>();

        public virtual ICollection<FavouritePost> FavouritePosts { get; set; } = new HashSet<FavouritePost>();

        public virtual ICollection<RecommendedFriend> RecommendedFriends { get; set; } = new HashSet<RecommendedFriend>();
    }
}
