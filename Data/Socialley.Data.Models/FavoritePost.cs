namespace Socialley.Data.Models
{
    using System;

    using Socialley.Data.Common.Models;

    public class FavoritePost : BaseDeletableModel<string>
    {
        public FavoritePost()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string PostId { get; set; }

        public virtual Post Post { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
