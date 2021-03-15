namespace Socialley.Data.Models
{
    using System;

    using Socialley.Data.Common.Models;

    public class UserFollower : IAuditInfo, IDeletableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string FollowerId { get; set; }

        public virtual ApplicationUser Follower { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
