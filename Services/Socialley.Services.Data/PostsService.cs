namespace Socialley.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;
    using Socialley.Web.ViewModels.Posts;

    using Ganss.XSS;


    public class PostsService : IPostsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg", "PNG" };

        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<UserImage> imagesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<UserImage> imagesRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.postsRepository = postsRepository;
            this.imagesRepository = imagesRepository;
            this.usersRepository = usersRepository;
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
            var posts = this.postsRepository
                .All()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new PostViewModel
                {
                    UserUserName = x.User.UserName,
                    ImageUrl = "/images/posts/" + x.ImagePosts.FirstOrDefault(x => x.UserId == userId).Id + "." + x.ImagePosts.FirstOrDefault(x => x.UserId == userId).Extension,
                    Content = x.Content,
                    UserProfileImageUrl = (x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ?
                    "/images/users/" + x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + x.User.UserImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : "/images/users/default-profile-icon.jpg",
                    CreatedOn = x.CreatedOn,
                })
                .ToArray();

            return posts;
        }
    }
}
