namespace Socialley.Web.ViewModels.Comments
{
    using Ganss.XSS;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CommentViewModel
    {
        public string CommentId { get; set; }

        public string PostId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserProfileUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}
