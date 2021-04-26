namespace Socialley.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using Ganss.XSS;

    public class UserProfileViewModel
    {
        public string UserName { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string ProfileImageUrl { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingsCount { get; set; }

        public int PostsCount { get; set; }

        public IList<UserPostsViewModel> UserPosts { get; set; } = new List<UserPostsViewModel>();

        public IList<UserFollowingsViewModel> UserFollowings { get; set; }

        public IList<UserFollowersViewModel> UserFollowers { get; set; }
    }
}
