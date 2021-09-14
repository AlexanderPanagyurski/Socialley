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

        [Authorize]
        public IActionResult Edit(string id)
        {
            var postViewModel = this.postsService.EditPost(id);
            if (this.userManager.GetUserId(this.User) != postViewModel.OwnerId
                && !this.User.IsInRole(Socialley.Common.GlobalConstants.AdministratorRoleName))
            {
                this.TempData["InfoMessage"] = "You are not authorized to edit this post.";
                return this.Redirect("/Home/Index");
            }

            return this.View(postViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string id, EditPostViewModel post)
        {
            var postViewModel = this.postsService.EditPost(id);
            if (this.userManager.GetUserId(this.User) != postViewModel.OwnerId
                && !this.User.IsInRole(Socialley.Common.GlobalConstants.AdministratorRoleName))
            {
                this.TempData["InfoMessage"] = "You are not authorized to edit this post.";
                return this.Redirect("/Home/Index");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.postsService.UpdateAsync(id, post);
            this.TempData["InfoMessage"] = "Successfully edited post.";
            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var post = this.postsService.GetById(id, user.Id);
            if (this.userManager.GetUserId(this.User) != post.UserId
                && !this.User.IsInRole(Socialley.Common.GlobalConstants.AdministratorRoleName))
            {
                this.TempData["InfoMessage"] = "You are not authorized to delete this post.";
                return this.Redirect("/Home/Index");
            }

            await this.postsService.DeleteAsync(id);
            this.TempData["InfoMessage"] = "Successfully deleted post.";
            return this.Redirect("/Home/Index");
        }
    }
}
