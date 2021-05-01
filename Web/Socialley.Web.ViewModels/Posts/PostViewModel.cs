namespace Socialley.Web.ViewModels.Posts
{
    using System;

    using Ganss.XSS;

    public class PostViewModel
    {
        public string Id { get; set; }

        public int FavoritesCount { get; set; }

        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public string UserProfileImageUrl { get; set; }

        public int UserFollowingsCount { get; set; }

        public int UserFollowersCount { get; set; }

        public int UserPostsCount { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public bool IsLiked { get; set; }
    }
}
