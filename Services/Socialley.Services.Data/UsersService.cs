﻿namespace Socialley.Services.Data
{
    using Microsoft.AspNetCore.Identity;
    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;
    using Socialley.Web.ViewModels.Users;
    using System.Linq;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Post> postsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.postsRepository = postsRepository;
            this.userManager = userManager;
        }

        public AllUsersViewModel GetAllUsers(int? take = null, int skip = 0)
        {
            AllUsersViewModel viewModel = null;

            if (take.HasValue)
            {
                viewModel = new AllUsersViewModel
                {
                    AllUsers = this.usersRepository
                        .All()
                        .OrderByDescending(x => x.Posts.Count())
                        .Select(x => new UserViewModel
                        {
                            Email = x.Email,
                            PostsCount = x.Posts.Count(),
                            ProfileImageUrl = (x.UserImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ? "/images/users/" + x.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + x.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg",
                            UserId = x.Id,
                            UserUserName = x.UserName,
                        })
                        .Skip(skip)
                        .Take(take.Value)
                        .ToArray(),
                };
            }
            else
            {
                viewModel = new AllUsersViewModel
                {
                    AllUsers = this.usersRepository
                        .All()
                        .Select(x => new UserViewModel
                        {
                            Email = x.Email,
                            PostsCount = x.Posts.Count(),
                            ProfileImageUrl = (x.UserImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ? "/images/users/" + x.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + x.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg",
                            UserId = x.Id,
                            UserUserName = x.UserName,
                        })
                        .ToArray(),
                };
            }

            return viewModel;
        }

        public int GetUsersCount()
        {
            return this.usersRepository.All().Count();
        }
    }
}
