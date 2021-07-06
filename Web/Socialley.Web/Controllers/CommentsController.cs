namespace Socialley.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Socialley.Data.Models;
    using Socialley.Services.Data;
    using Socialley.Web.ViewModels.Comments;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(
            ICommentsService commentsService,
            UserManager<ApplicationUser> userManager)
        {
            this.commentsService = commentsService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.commentsService.Create(input.PostId, userId, input.Content, input.ParentId);

            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }
    }
}