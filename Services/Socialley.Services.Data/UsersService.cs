namespace Socialley.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;
    using Socialley.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg", "PNG", "JPG", "JPEG", "GIF" };

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

        public async Task ChangeAvatar(string userId, AvatarEditInputModel input, string imagePath)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            Directory.CreateDirectory($"{imagePath}/users/");
            if (input.NewProfileImage != null)
            {
                var image = input.NewProfileImage;
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new ArgumentException($"Invalid image extension {extension}");
                }

                var oldProfileImage = this.userImagesRepository.All().FirstOrDefault(x => x.UserId == userId && x.IsProfileImage);
                if (oldProfileImage != null)
                {
                    oldProfileImage.IsProfileImage = false;
                }

                var dbImage = new UserImage
                {
                    UserId = userId,
                    Extension = extension,
                    IsProfileImage = true,
                };
                user.UserImages.Add(dbImage);
                var physicalPath = $"{imagePath}/users/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
                await this.userImagesRepository.SaveChangesAsync();
            }
        }

        public async Task FollowUserAsync(string followerId, string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var follower = this.usersRepository.All().FirstOrDefault(x => x.Id == followerId);

            user.Followers.Add(new UserFollower { UserId = follower.Id, FollowerId = user.Id });
            await this.usersRepository.SaveChangesAsync();
        }

        public AllUsersViewModel GetAllUsers(string userId, int? take = null, int skip = 0)
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
                            IsFollowed = this.followersRepository.All().FirstOrDefault(y => y.FollowerId == userId && y.UserId == x.Id) != null,
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
                            IsFollowed = this.followersRepository.All().FirstOrDefault(y => y.FollowerId == userId && y.UserId == x.Id) != null,
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
            var userFavouritePosts = this.userFavouritePostsRepository.
                All().
                Where(x => x.UserId == userId).
                OrderByDescending(x => x.CreatedOn);
            var userFollowings = this.followersRepository.All().Where(x => x.FollowerId == userId);
            var userFollowers = this.followersRepository.All().Where(x => x.UserId == userId);

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
            viewModel.UserFollowings = new List<UserFollowingsViewModel>();
            viewModel.UserFollowers = new List<UserFollowersViewModel>();

            foreach (var userPost in userFavouritePosts)
            {
                var currUserPostLikes = this.userFavouritePostsRepository.All().Count(x => x.PostId == userPost.PostId);
                viewModel.UserPosts.Add(new UserPostsViewModel
                {
                    ImageUrl = "/images/posts/" + userPostsImages.FirstOrDefault(x => x.PostId == userPost.PostId).Id + "." + userPostsImages.FirstOrDefault(x => x.PostId == userPost.PostId).Extension,
                    PostsId = userPost.PostId,
                    PostLikes = currUserPostLikes,
                });
            }

            foreach (var userFollowing in userFollowings)
            {
                var currUser = this.usersRepository.All().FirstOrDefault(x => x.Id == userFollowing.UserId);
                viewModel.UserFollowings.Add(new UserFollowingsViewModel
                {
                    Id = currUser.Id,
                    UserName = currUser.UserName,
                    UserProfileUrl = (this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == currUser.Id) != null) ? "/images/users/" + this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == currUser.Id).Id + "." +
                this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == currUser.Id).Extension : "/images/users/default-profile-icon.jpg",
                });
            }

            foreach (var userFollower in userFollowers)
            {
                var currUser = this.usersRepository.All().FirstOrDefault(x => x.Id == userFollower.FollowerId);
                viewModel.UserFollowers.Add(new UserFollowersViewModel
                {
                    Id = currUser.Id,
                    UserName = currUser.UserName,
                    UserProfileUrl = (this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == userFollower.FollowerId) != null) ? "/images/users/" + this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == userFollower.FollowerId).Id + "." +
                this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == userFollower.FollowerId).Extension : "/images/users/default-profile-icon.jpg",
                });
            }

            return viewModel;
        }

        public UserProfileViewModel GetUserProfile(string userId)
        {
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);
            var userPostsImages = this.postsImagesRepository.All().Where(x => x.UserId == userId);
            var userPosts = this.postsRepository
                .All()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn);
            var userImages = this.userImagesRepository.All().Where(x => x.UserId == userId);
            var userFollowings = this.followersRepository.All().Where(x => x.FollowerId == userId);
            var userFollowers = this.followersRepository.All().Where(x => x.UserId == userId);

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
            viewModel.UserFollowings = new List<UserFollowingsViewModel>();
            viewModel.UserFollowers = new List<UserFollowersViewModel>();

            foreach (var userPost in userPosts)
            {
                var currUserPostLikes = this.userFavouritePostsRepository.All().Count(x => x.PostId == userPost.Id);
                viewModel.UserPosts.Add(new UserPostsViewModel
                {
                    ImageUrl = "/images/posts/" + userPostsImages.FirstOrDefault(x => x.PostId == userPost.Id).Id + "." + userPostsImages.FirstOrDefault(x => x.PostId == userPost.Id).Extension,
                    PostsId = userPost.Id,
                    PostLikes = currUserPostLikes,
                });
            }

            foreach (var userFollowing in userFollowings)
            {
                var currUser = this.usersRepository.All().FirstOrDefault(x => x.Id == userFollowing.UserId);
                viewModel.UserFollowings.Add(new UserFollowingsViewModel
                {
                    Id = currUser.Id,
                    UserName = currUser.UserName,
                    UserProfileUrl = (this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == currUser.Id) != null) ? "/images/users/" + this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == currUser.Id).Id + "." +
                this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == currUser.Id).Extension : "/images/users/default-profile-icon.jpg",
                    IsFollowed = true,
                });
            }

            foreach (var userFollower in userFollowers)
            {
                var currUser = this.usersRepository.All().FirstOrDefault(x => x.Id == userFollower.FollowerId);
                viewModel.UserFollowers.Add(new UserFollowersViewModel
                {
                    Id = currUser.Id,
                    UserName = currUser.UserName,
                    UserProfileUrl = (this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == userFollower.FollowerId) != null) ? "/images/users/" + this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == userFollower.FollowerId).Id + "." +
                this.userImagesRepository.All().FirstOrDefault(x => x.IsProfileImage == true && x.UserId == userFollower.FollowerId).Extension : "/images/users/default-profile-icon.jpg",
                    IsFollowed = this.followersRepository.All().FirstOrDefault(x => x.FollowerId == userId && x.UserId == currUser.Id) != null,
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
