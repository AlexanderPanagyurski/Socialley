namespace Socialley.Web.ViewModels.Users
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public string UserName { get; set; }

        public IEnumerable<UserViewModel> AllUsers { get; set; }
    }
}
