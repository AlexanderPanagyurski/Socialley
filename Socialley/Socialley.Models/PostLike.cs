using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Models
{
    public class PostLike
    {

        public string UserId { get; set; }
        public virtual User User { get; set; }


        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public bool IsLiked { get; set; }
    }
}
