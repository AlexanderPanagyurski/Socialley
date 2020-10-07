using Socialley.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class Comment
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        [Required]
        [MaxLength(AttributesConstraints.CarCommentUserFullNameMaxLength)]
        public string UserFullName { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.ContentMaxLength)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey(nameof(Comment))]
        public string ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<Comment> CommentReplies { get; set; } = new HashSet<Comment>();
    }
}
