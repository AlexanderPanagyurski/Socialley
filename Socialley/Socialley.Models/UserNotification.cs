using Socialley.Common;
using Socialley.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class UserNotification
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        public NotificationType NotificationType { get; set; }

        public bool IsRead { get; set; }


        [Required]
        [MaxLength(AttributesConstraints.NameMaxLength)]
        public string TargetUserFirstName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.NameMaxLength)]
        public string TargetUserLastName { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
