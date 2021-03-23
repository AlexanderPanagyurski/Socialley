namespace Socialley.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;
    using Socialley.Web.ViewModels.Posts;

    public class PostsService : IPostsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg", "PNG" };

        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<UserImage> imagesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<UserFollower> followersRepository;
        private readonly IDeletableEntityRepository<ImagePost> postsImagesRepository;
        private readonly IDeletableEntityRepository<FavoritePost> postsLikesRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<UserImage> imagesRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<UserFollower> followersRepository,
            IDeletableEntityRepository<ImagePost> postsImagesRepository,
            IDeletableEntityRepository<FavoritePost> postsLikesRepository)
        {
            this.postsRepository = postsRepository;
            this.imagesRepository = imagesRepository;
            this.usersRepository = usersRepository;
            this.followersRepository = followersRepository;
            this.postsImagesRepository = postsImagesRepository;
            this.postsLikesRepository = postsLikesRepository;
        }

        public async Task<string> CreateAsync(PostCreateInputModel input, string userId, string imagePath)
        {
            var post = new Post
            {
                Content = input.Content,
                UserId = userId,
            };
            Directory.CreateDirectory($"{imagePath}/posts/");
            if (input.Images != null)
            {
                foreach (var image in input.Images)
                {
                    var extension = Path.GetExtension(image.FileName).TrimStart('.');
                    if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                    {
                        throw new ArgumentException($"Invalid image extension {extension}");
                    }

                    var dbImage = new ImagePost
                    {
                        UserId = userId,
                        Post = post,
                        Extension = extension,
                    };
                    post.ImagePosts.Add(dbImage);
                    var physicalPath = $"{imagePath}/posts/{dbImage.Id}.{extension}";
                    using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                    await image.CopyToAsync(fileStream);
                }
            }

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
            return post.Id;
        }

        public PostViewModel[] GetPosts(string userId)
        {
            var followings = this.followersRepository.All().Where(x => x.FollowerId == userId);
            List<PostViewModel> posts = new List<PostViewModel>();

            foreach (var user in followings)
            {
                var currUser = this.usersRepository.All().FirstOrDefault(x => x.Id == user.UserId);
                var currUserPosts = this.postsRepository.All().Where(x => x.UserId == currUser.Id);
                foreach (var post in currUserPosts)
                {
                    var currPostImage = this.postsImagesRepository.All().Where(x => x.PostId == post.Id);
                    var postOwner = this.usersRepository.All().FirstOrDefault(x => x.Id == post.UserId);
                    var postOwnerImages = this.imagesRepository.All().Where(x => x.UserId == postOwner.Id);
                    var postOwnerPorifleImag = (postOwnerImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ?
                        "/images/users/" + postOwnerImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + postOwnerImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : " /images/users/default-profile-icon.jpg";
                    var postLikesCount = this.postsLikesRepository.All().Count(x => x.PostId == post.Id);

                    posts.Add(new PostViewModel
                    {
                        Id = post.Id,
                        FavoritesCount = postLikesCount,
                        UserUserName = currUser.UserName,
                        ImageUrl = "/images/posts/" + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Id + "." + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Extension,
                        Content = post.Content,
                        UserProfileImageUrl = postOwnerPorifleImag,
                        CreatedOn = post.CreatedOn,
                        IsLiked = this.postsLikesRepository.All().Any(x => x.PostId == post.Id && x.UserId == userId),
                    });
                }
            }

            return posts.OrderByDescending(x => x.CreatedOn).ToArray();
        }
    }
}
