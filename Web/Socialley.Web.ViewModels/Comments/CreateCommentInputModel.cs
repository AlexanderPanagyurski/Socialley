namespace Socialley.Web.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Socialley.Data.Models;

    public class CreateCommentInputModel
    {
        public string PostId { get; set; }

        public string Content { get; set; }

        public string ParentId { get; set; }

    }
}
