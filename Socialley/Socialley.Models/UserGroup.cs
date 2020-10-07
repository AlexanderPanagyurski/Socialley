using System;
using System.Collections.Generic;
using System.Text;

namespace Socialley.Models
{
    public class UserGroup
    {
        public string GroupId { get; set; }
        public virtual Group Group { get; set; }


        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
