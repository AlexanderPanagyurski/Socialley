﻿namespace Socialley.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserProfileViewModel
    {
        public string ProfileImageUrl { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingsCount { get; set; }

        public int PostsCount { get; set; }

        public IList<UserPostsViewModel> UserPosts { get; set; } = new List<UserPostsViewModel>();
    }
}
