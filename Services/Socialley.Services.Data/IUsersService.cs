namespace Socialley.Services.Data
{
    using Socialley.Web.ViewModels.Users;

    public interface IUsersService
    {
        AllUsersViewModel GetAllUsers(int? take = null, int skip = 0);

        int GetUsersCount();
    }
}
