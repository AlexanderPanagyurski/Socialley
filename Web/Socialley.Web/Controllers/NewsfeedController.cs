namespace Socialley.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Socialley.Services.Data;
    using Socialley.Web.ViewModels.Posts;

    [ApiController]
    [Route("api/[controller]")]
    public class NewsfeedController : BaseController
    {
        private readonly IPostsService postsService;

        public NewsfeedController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public ActionResult<PostViewModel[]> AllPosts(string title)
        {
            var responseModel = this.postsService.GetAllPosts(title);

            return responseModel;
        }
    }
}
