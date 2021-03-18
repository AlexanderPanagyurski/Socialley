using System.Collections.Generic;

namespace Socialley.Web.ViewModels.Users
{
    public class AllUsersViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<UserViewModel> AllUsers { get; set; }
    }
}
