using Socialley.Data.Common.Repositories;
using Socialley.Data.Models;
using Socialley.Web.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socialley.Services.Data
{
    public class SearchesService : ISearchesService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public SearchesService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public SearchedUsersResponseModel[] Searches(string title)
        {
            SearchedUsersResponseModel[] titles = this.usersRepository
                .All()
                .Where(x => x.UserName.Contains(title))
                .Select(x => new SearchedUsersResponseModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    ProfileImageUrl = (x.UserImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ? "/images/users/" + x.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + x.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg",
                })
                .ToArray();

            return titles;
        }
    }
}
