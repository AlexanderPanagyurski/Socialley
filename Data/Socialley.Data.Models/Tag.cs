namespace Socialley.Data.Models
{
    using Socialley.Data.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Tag : BaseDeletableModel<string>
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }


        public virtual ICollection<PostTag> TagsPosts { get; set; } = new HashSet<PostTag>();
    }
}
