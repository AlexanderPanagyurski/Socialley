namespace Socialley.Web.ViewModels.Comments
{
    using Ganss.XSS;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    public class CommentViewModel
    {
        public string CommentId { get; set; }

        public string PostId { get; set; }

        public string ParentId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserProfileUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

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
    }
}
