namespace Socialley.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.AspNetCore.Http;

    public class EditPostViewModel
    {
        public string PostId { get; set; }

        public string OwnerId { get; set; }

        public string Content { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
