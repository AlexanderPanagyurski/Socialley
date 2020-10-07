using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Models
{
    public class PostTag
    {
        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public string TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
