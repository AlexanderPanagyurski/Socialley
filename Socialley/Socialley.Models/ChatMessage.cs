using Socialley.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class ChatMessage
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(User))]
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        [Required]
        public DateTime SendOn { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.MessageTextMaxLength)]
        public string Content { get; set; }
    }
}
