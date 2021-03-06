﻿namespace Socialley.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Socialley.Data.Common.Models;

    public class Comment : BaseDeletableModel<string>
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Content { get; set; }

        public string PostId { get; set; }

        public virtual Post Post { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ParentId { get; set; }

        public virtual Comment Parent { get; set; }
    }
}
