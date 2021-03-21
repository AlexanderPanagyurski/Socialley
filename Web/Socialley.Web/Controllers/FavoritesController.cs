namespace Socialley.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Socialley.Data.Models;
    using Socialley.Services.Data;
    using Socialley.Web.ViewModels.FavouritesPosts;

    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService favoritesService;
        private readonly UserManager<ApplicationUser> userManager;

        public FavoritesController(
            IFavoritesService favoritesService,
            UserManager<ApplicationUser> userManager)
        {
            this.favoritesService = favoritesService;
            this.userManager = userManager;
            this.userManager = userManager;
            this.favoritesService = favoritesService;
        }

        [Authorize]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<FavoritePostResponseModel>> Post(FavoritePostInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.favoritesService.FavorAsync(input.PostId, userId);
            var favoritesCount = this.favoritesService.GetCount(input.PostId);
            return new FavoritePostResponseModel { FavoritesCount = favoritesCount };
        }
    }
}
