namespace Socialley.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Socialley.Data.Models;
    using Socialley.Services.Data;
    using Socialley.Web.ViewModels.Posts;

    public class PostsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly IPostsService postsService;

        public PostsController(
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            IPostsService postsService)
        {
            this.userManager = userManager;
            this.environment = environment;
            this.postsService = postsService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCreateInputModel input)
        {
            var id = string.Empty;

            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid || input.Content == null)
            {
                return this.RedirectToAction(nameof(this.Create));
            }

            try
            {
                id = await this.postsService.CreateAsync(input, user.Id, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.RedirectToAction(nameof(this.Create));
            }

            this.TempData["InfoMessage"] = "Forum post created!";
            return this.RedirectToAction(nameof(Index), new { Controller = "Home" });
        }

        public async Task<IActionResult> ById(string id)
        {
            var loggedUser = await this.userManager.GetUserAsync(this.User);
            var postViewModel = this.postsService.GetById(id, loggedUser.Id);

            if (postViewModel == null)
            {
                return this.Redirect("Home/StatusCodeError");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            return this.View(postViewModel);
        }
    }
}
