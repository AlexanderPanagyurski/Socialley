using Socialley.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Socialley.Models
{
   public  class Tag
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        [Required,MaxLength(20)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();
    }
}
