namespace Socialley.Services.Data
{
    using System.Threading.Tasks;

    using Socialley.Web.ViewModels.Users;

    public interface IUsersService
    {
        AllUsersViewModel GetAllUsers(int? take = null, int skip = 0);

        int GetUsersCount();

        Task FollowUserAsync(string followerId, string userId);

        UserProfileViewModel GetUserProfile(string userId);
    }
}
