namespace Socialley.Data.Models
{
    using Socialley.Data.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class PostTag: BaseDeletableModel<string>
    {
        [Required]
        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        public Post Post { get; set; }

        [Required]
        [ForeignKey(nameof(Tag))]
        public string TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
