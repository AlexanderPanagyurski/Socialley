﻿namespace Socialley.Web.ViewModels.Posts
{
    using System;

    using Ganss.XSS;

    public class PostViewModel
    {
        public string UserUserName { get; set; }

        public string UserProfileImageUrl { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }
    }
}