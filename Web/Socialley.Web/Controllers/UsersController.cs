namespace Socialley.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Socialley.Data.Models;
    using Socialley.Services.Data;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(
            IUsersService usersService,
            IPostsService postsService,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.postsService = postsService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult GetAllUsers(int page = 1)
        {
            var viewModel = this.usersService.GetAllUsers(6, (page - 1) * 6);
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
        public async Task<IActionResult> UserProfile()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.usersService.GetUserProfile(user.Id);
            return this.View(viewModel);
        }

        public async Task<IActionResult> FavouritePosts()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.usersService.GetFavouritePosts(user.Id);

            return this.View(viewModel);
        }
    }
}
