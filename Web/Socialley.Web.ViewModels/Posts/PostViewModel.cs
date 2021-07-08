namespace Socialley.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;
    using Ganss.XSS;
    using Socialley.Web.ViewModels.Comments;

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

        public string ShortContent
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Content, @"<[^>]+>", string.Empty));
                return content.Length > 300
                        ? content.Substring(0, 300) + "..."
                        : content;
            }
        }

        public string SanitizedShortContent => new HtmlSanitizer().Sanitize(this.ShortContent);

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public bool IsLiked { get; set; }

        public string LastSixUserPostsUrls { get; set; }

        public IEnumerable<CommentViewModel> PostComments { get; set; }
    }
}
