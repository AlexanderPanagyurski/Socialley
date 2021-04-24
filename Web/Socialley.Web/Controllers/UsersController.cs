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
    using Socialley.Web.ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;

        public UsersController(
            IUsersService usersService,
            IPostsService postsService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            this.usersService = usersService;
            this.postsService = postsService;
            this.userManager = userManager;
            this.environment = environment;
        }

        [Authorize]
        public async Task<IActionResult> GetAllUsers(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.usersService.GetAllUsers(user.Id,6, (page - 1) * 6);
            var count = this.usersService.GetUsersCount();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / 6);
            viewModel.CurrentPage = page;
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Follow(string userId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.usersService.FollowUserAsync(userId, user.Id);

            return this.Redirect(nameof(this.GetAllUsers));
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.usersService.GetUserProfile(user.Id);
            return this.View(viewModel);
        }

        public IActionResult UserProfile(string userId)
        {
            var viewModel = this.usersService.GetUserProfile(userId);
            return this.View(viewModel);
        }

        public async Task<IActionResult> FavouritePosts()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.usersService.GetFavouritePosts(user.Id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvatar(string userId, AvatarEditInputModel model)
        {
            if (this.userManager.GetUserId(this.User) != userId
                && !this.User.IsInRole(Socialley.Common.GlobalConstants.AdministratorRoleName))
            {
                return this.Redirect("/Home/Index");
            }

            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Identity/Account/Manage/ChangeAvatar");
            }

            try
            {
                await this.usersService.ChangeAvatar(userId, model, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                this.TempData["Message"] = "This format is not supproted. Please try again.";
                return this.Redirect("/Identity/Account/Manage/ChangeAvatar");
            }

            this.TempData["Message"] = "Successfully updated your profile!";
            return this.Redirect("/Identity/Account/Manage/ChangeAvatar");
        }
    }
}
