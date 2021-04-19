namespace Socialley.Web.ViewModels.Users
{
    using Microsoft.AspNetCore.Http;

    public class AvatarEditInputModel
    {
        public string UserId { get; set; }

        public IFormFile NewProfileImage { get; set; }
    }
}
