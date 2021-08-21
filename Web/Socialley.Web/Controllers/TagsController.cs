namespace Socialley.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Socialley.Services.Data;
    using System.Threading.Tasks;

    public class TagsController : BaseController
    {
        private readonly ITagsService tagsService;

        public TagsController(ITagsService tagsService)
        {
            this.tagsService = tagsService;
        }

        public async Task<IActionResult> TagPosts(string tagName)
        {
            var viewModel = await this.tagsService.GetTagsPostsAsync(tagName);

            return this.View(viewModel);
        }
    }
}
