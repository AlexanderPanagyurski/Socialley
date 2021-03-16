namespace Socialley.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public class PostCreateInputModel
    {
        public string Content { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
