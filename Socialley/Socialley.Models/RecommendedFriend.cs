using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class RecommendedFriend
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RecommendedUsername { get; set; }

        [MaxLength(15)]
        public string RecommendedFirstName { get; set; }

        [MaxLength(15)]
        public string RecommendedLastName { get; set; }

        [Required]
        public string RecommendedImageUrl { get; set; }

        [Required]
        public string RecommendedCoverImage { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
