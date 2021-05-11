using Socialley.Web.ViewModels.Users;

namespace Socialley.Services.Data
{
    public interface ISearchesService
    {
        SearchedUsersResponseModel[] Searches(string title);
    }
}
