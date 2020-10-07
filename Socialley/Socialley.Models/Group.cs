using Socialley.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class Group
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        [Required,MaxLength(AttributesConstraints.GroupNameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(ChatMessage))]
        public string ChatMessageId { get; set; }
        public virtual ChatMessage ChatMessage { get; set; }


        public virtual ICollection<UserGroup> UsersGroups { get; set; } = new HashSet<UserGroup>();
    }
}
