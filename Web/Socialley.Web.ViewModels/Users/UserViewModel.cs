namespace Socialley.Web.ViewModels.Users
{
    public class UserViewModel
    {
        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public string ProfileImageUrl { get; set; }

        public int PostsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingsCount { get; set; }

        public string Email { get; set; }

        public bool IsFollowed { get; set; }
    }
}
