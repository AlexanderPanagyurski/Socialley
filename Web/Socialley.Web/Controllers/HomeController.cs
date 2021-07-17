namespace Socialley.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Socialley.Data.Models;
    using Socialley.Services.Data;
    using Socialley.Web.ViewModels;
    using Socialley.Web.ViewModels.Posts;

    public class HomeController : BaseController
    {
        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IPostsService postsService,
            UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            PostViewModel[] viewModel = new PostViewModel[0];
            if (user != null)
            {
                viewModel = this.postsService.GetPosts(user.Id);
            }

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult StatusCodeError(int errorCode)
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
