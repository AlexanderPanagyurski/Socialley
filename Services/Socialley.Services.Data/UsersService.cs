namespace Socialley.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;
    using Socialley.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<UserFollower> followersRepository;
        private readonly IDeletableEntityRepository<ImagePost> postsImagesRepository;
        private readonly IDeletableEntityRepository<UserImage> userImagesRepository;
        private readonly IDeletableEntityRepository<FavoritePost> userFavouritePostsRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Post> postsRepository,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<UserFollower> followersRepository,
            IDeletableEntityRepository<ImagePost> postsImagesRepository,
            IDeletableEntityRepository<UserImage> userImagesRepository,
            IDeletableEntityRepository<FavoritePost> userFavouritePostsRepository)
        {
            this.usersRepository = usersRepository;
            this.postsRepository = postsRepository;
            this.userManager = userManager;
            this.followersRepository = followersRepository;
            this.postsImagesRepository = postsImagesRepository;
            this.userImagesRepository = userImagesRepository;
            this.userFavouritePostsRepository = userFavouritePostsRepository;
        }

        public async Task FollowUserAsync(string followerId, string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var follower = this.usersRepository.All().FirstOrDefault(x => x.Id == followerId);

            user.Followers.Add(new UserFollower { UserId = follower.Id, FollowerId = user.Id });
            await this.usersRepository.SaveChangesAsync();
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
                            FollowersCount = this.followersRepository.All().Count(y => y.UserId == x.Id),
                            FollowingsCount = this.followersRepository.All().Count(y => y.FollowerId == x.Id),
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
                            FollowersCount = this.followersRepository.All().Count(y => y.UserId == x.Id),
                            FollowingsCount = this.followersRepository.All().Count(y => y.FollowerId == x.Id),
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

        public UserProfileViewModel GetFavouritePosts(string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var userPostsImages = this.postsImagesRepository.All();
            var userImages = this.userImagesRepository.All().Where(x => x.UserId == userId);
            var userFavouritePosts = this.userFavouritePostsRepository.All().Where(x => x.UserId == userId);

            var viewModel = new UserProfileViewModel
            {
                UserName = user.UserName,
                PostsCount = this.postsRepository.All().Count(x => x.UserId == userId),
                FollowersCount = this.followersRepository.All().Count(y => y.UserId == userId),
                FollowingsCount = this.followersRepository.All().Count(y => y.FollowerId == userId),
                ProfileImageUrl = (userImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ? "/images/users/" + userImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." +
                userImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg",
            };
            viewModel.UserPosts = new List<UserPostsViewModel>();

            foreach (var userPost in userFavouritePosts)
            {
                viewModel.UserPosts.Add(new UserPostsViewModel
                {
                    ImageUrl = "/images/posts/" + userPostsImages.FirstOrDefault(x => x.PostId == userPost.PostId).Id + "." + userPostsImages.FirstOrDefault(x => x.PostId == userPost.PostId).Extension,
                    PostsId = userPost.PostId,
                });
            }

            return viewModel;
        }

        public UserProfileViewModel GetUserProfile(string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var userPostsImages = this.postsImagesRepository.All().Where(x => x.UserId == userId);
            var userPosts = this.postsRepository.All().Where(x => x.UserId == userId);
            var userImages = this.userImagesRepository.All().Where(x => x.UserId == userId);

            var viewModel = new UserProfileViewModel
            {
                UserName = user.UserName,
                PostsCount = this.postsRepository.All().Count(x => x.UserId == userId),
                FollowersCount = this.followersRepository.All().Count(y => y.UserId == userId),
                FollowingsCount = this.followersRepository.All().Count(y => y.FollowerId == userId),
                ProfileImageUrl = (userImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ? "/images/users/" + userImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." +
                userImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg",
            };
            viewModel.UserPosts = new List<UserPostsViewModel>();

            foreach (var userPost in userPosts)
            {
                viewModel.UserPosts.Add(new UserPostsViewModel
                {
                    ImageUrl = "/images/posts/" + userPostsImages.FirstOrDefault(x => x.PostId == userPost.Id).Id + "." + userPostsImages.FirstOrDefault(x => x.PostId == userPost.Id).Extension,
                    PostsId = userPost.Id,
                });
            }

            return viewModel;
        }

        public int GetUsersCount()
        {
            return this.usersRepository.All().Count();
        }
    }
}
