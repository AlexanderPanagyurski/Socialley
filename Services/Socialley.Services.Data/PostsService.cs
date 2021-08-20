namespace Socialley.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;
    using Socialley.Web.ViewModels.Comments;
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
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<UserImage> userImagesRepository;
        private readonly IDeletableEntityRepository<Tag> tagsRepository;
        private readonly IDeletableEntityRepository<PostTag> postsTagsRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            IDeletableEntityRepository<UserImage> imagesRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<UserFollower> followersRepository,
            IDeletableEntityRepository<ImagePost> postsImagesRepository,
            IDeletableEntityRepository<FavoritePost> postsLikesRepository,
            IDeletableEntityRepository<Comment> commentsRepository,
            IDeletableEntityRepository<UserImage> userImagesRepository,
            IDeletableEntityRepository<Tag> tagsRepository,
            IDeletableEntityRepository<PostTag> postsTagsRepository)
        {
            this.postsRepository = postsRepository;
            this.imagesRepository = imagesRepository;
            this.usersRepository = usersRepository;
            this.followersRepository = followersRepository;
            this.postsImagesRepository = postsImagesRepository;
            this.postsLikesRepository = postsLikesRepository;
            this.commentsRepository = commentsRepository;
            this.userImagesRepository = userImagesRepository;
            this.tagsRepository = tagsRepository;
            this.postsTagsRepository = postsTagsRepository;
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

            if (input.Tags != null)
            {
                var tagsArray = input.Tags.Split(',');
                foreach (var tag in tagsArray)
                {
                    Tag currTag = null;

                    if (this.tagsRepository.All().Any(t => t.Name == tag))
                    {
                        currTag = this.tagsRepository.All().FirstOrDefault(t => t.Name == tag);
                    }
                    else
                    {
                        currTag = new Tag { Name = tag };
                        await this.tagsRepository.AddAsync(currTag);
                        await this.tagsRepository.SaveChangesAsync();
                    }

                    await this.postsTagsRepository.AddAsync(new PostTag { PostId = post.Id, TagId = currTag.Id });
                    await this.postsTagsRepository.SaveChangesAsync();
                }
            }

            return post.Id;
        }

        public EditPostViewModel EditPost(string id)
        {
            var post = this.postsRepository.All().FirstOrDefault(x => x.Id == id);
            EditPostViewModel viewModel = new EditPostViewModel
            {
                PostId = post.Id,
                OwnerId = post.UserId,
                Content = post.Content,
            };

            return viewModel;
        }

        public PostViewModel[] GetAllPosts(string userId)
        {
            var users = this.usersRepository.All();

            List<PostViewModel> posts = new List<PostViewModel>();

            foreach (var user in users)
            {
                var currUser = this.usersRepository.All().FirstOrDefault(x => x.Id == user.Id);
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
                        UserId = currUser.Id,
                        FavoritesCount = postLikesCount,
                        UserUserName = currUser.UserName,
                        ImageUrl = "/images/posts/" + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Id + "." + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Extension,
                        Content = post.Content,
                        UserProfileImageUrl = postOwnerPorifleImag,
                        CreatedOn = post.CreatedOn,
                        ModifiedOn = post.ModifiedOn,
                        IsLiked = this.postsLikesRepository.All().Any(x => x.PostId == post.Id && x.UserId == userId),
                        UserFollowersCount = this.followersRepository.All().Count(y => y.UserId == currUser.Id),
                        UserFollowingsCount = this.followersRepository.All().Count(y => y.FollowerId == currUser.Id),
                        UserPostsCount = this.postsRepository.All().Count(x => x.UserId == currUser.Id),
                    });
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='row'>");
                foreach (var currPostUrl in currUserPosts.OrderByDescending(x => x.CreatedOn).Take(6))
                {
                    sb.Append($"<div class='post-galley col-sm-4 mt-4 col-lg-4 col-md-12 thumb'><img src='{"/images/posts/" + currPostUrl.ImagePosts.FirstOrDefault(x => x.PostId == currPostUrl.Id).Id + "." + currPostUrl.ImagePosts.FirstOrDefault(x => x.PostId == currPostUrl.Id).Extension}' class='card-img rounded opacity img-fluid z-depth-1 pop'></div>");
                }

                sb.Append("</div>");

                foreach (var currPost in posts)
                {
                    if (currPost.UserId == currUser.Id)
                    {
                        currPost.LastSixUserPostsUrls = sb.ToString();
                    }
                }
            }

            return posts.OrderByDescending(x => x.CreatedOn).ToArray();
        }

        public PostViewModel GetById(string postId, string userId)
        {
            var post = this.postsRepository.All().FirstOrDefault(x => x.Id == postId);
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);

            var currPostImage = this.postsImagesRepository.All().Where(x => x.PostId == post.Id);
            var postOwner = this.usersRepository.All().FirstOrDefault(x => x.Id == post.UserId);
            var postOwnerImages = this.imagesRepository.All().Where(x => x.UserId == postOwner.Id);
            var postOwnerPorifleImag = (postOwnerImages.FirstOrDefault(x => x.IsProfileImage == true) != null) ?
                "/images/users/" + postOwnerImages.FirstOrDefault(x => x.IsProfileImage == true).Id + "." + postOwnerImages.FirstOrDefault(x => x.IsProfileImage == true).Extension : " /images/users/default-profile-icon.jpg";
            var postLikesCount = this.postsLikesRepository.All().Count(x => x.PostId == post.Id);

            PostViewModel viewModel = new PostViewModel
            {
                Id = post.Id,
                UserId = post.UserId,
                FavoritesCount = postLikesCount,
                UserUserName = postOwner.UserName,
                ImageUrl = "/images/posts/" + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Id + "." + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Extension,
                Content = post.Content,
                UserProfileImageUrl = postOwnerPorifleImag,
                CreatedOn = post.CreatedOn,
                ModifiedOn = post.ModifiedOn,
                IsLiked = this.postsLikesRepository.All().Any(x => x.PostId == post.Id && x.UserId == userId),
                PostComments = this.commentsRepository
                .All()
                .Where(x => x.PostId == post.Id)
                .Select(x => new CommentViewModel
                {
                    ParentId = x.ParentId,
                    CommentId = x.Id,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    UserId = x.UserId,
                    PostId = post.Id,
                    UserName = x.User.UserName,
                    UserProfileUrl = (this.userImagesRepository.All().FirstOrDefault(ui => ui.IsProfileImage == true && ui.UserId == x.UserId) != null) ?
                   "/images/users/" + this.userImagesRepository.All().FirstOrDefault(ui => ui.IsProfileImage == true && ui.UserId == x.UserId).Id + "." + this.userImagesRepository.All().FirstOrDefault(ui => ui.IsProfileImage == true && ui.UserId == x.UserId).Extension : "/images/users/default-profile-icon.jpg",
                })
                .OrderByDescending(x => x.CreatedOn)
                .ToList(),
                TagsNames = this.postsTagsRepository.All().Where(x => x.PostId == post.Id).Select(t => t.Tag.Name),
            };

            return viewModel;
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
                        UserId = currUser.Id,
                        FavoritesCount = postLikesCount,
                        UserUserName = currUser.UserName,
                        ImageUrl = "/images/posts/" + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Id + "." + currPostImage.FirstOrDefault(x => x.PostId == post.Id).Extension,
                        Content = post.Content,
                        UserProfileImageUrl = postOwnerPorifleImag,
                        CreatedOn = post.CreatedOn,
                        ModifiedOn = post.ModifiedOn,
                        IsLiked = this.postsLikesRepository.All().Any(x => x.PostId == post.Id && x.UserId == userId),
                        UserFollowersCount = this.followersRepository.All().Count(y => y.UserId == currUser.Id),
                        UserFollowingsCount = this.followersRepository.All().Count(y => y.FollowerId == currUser.Id),
                        UserPostsCount = this.postsRepository.All().Count(x => x.UserId == currUser.Id),
                        PostComments = this.commentsRepository
                .All()
                .Where(x => x.PostId == post.Id)
                .Select(x => new CommentViewModel
                {
                    ParentId = x.ParentId,
                    CommentId = x.Id,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    UserId = x.UserId,
                    PostId = post.Id,
                    UserName = x.User.UserName,
                    UserProfileUrl = (this.userImagesRepository.All().FirstOrDefault(ui => ui.IsProfileImage == true && ui.UserId == x.UserId) != null) ?
                   "/images/users/" + this.userImagesRepository.All().FirstOrDefault(ui => ui.IsProfileImage == true && ui.UserId == x.UserId).Id + "." + this.userImagesRepository.All().FirstOrDefault(ui => ui.IsProfileImage == true && ui.UserId == x.UserId).Extension : "/images/users/default-profile-icon.jpg",
                })
                .OrderByDescending(x => x.CreatedOn)
                .ToList(),
                        TagsNames = this.postsTagsRepository.All().Where(x => x.PostId == post.Id).Select(t => t.Tag.Name),
                    });
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='row'>");
                foreach (var currPostUrl in currUserPosts.OrderByDescending(x => x.CreatedOn).Take(6))
                {
                    sb.Append($"<div class='post-galley col-sm-4 mt-4 col-lg-4 col-md-12 thumb'><img src='{"/images/posts/" + currPostUrl.ImagePosts.FirstOrDefault(x => x.PostId == currPostUrl.Id).Id + "." + currPostUrl.ImagePosts.FirstOrDefault(x => x.PostId == currPostUrl.Id).Extension}' class='card-img rounded opacity img-fluid z-depth-1 pop'></div>");
                }

                sb.Append("</div>");

                foreach (var currPost in posts)
                {
                    if (currPost.UserId == currUser.Id)
                    {
                        currPost.LastSixUserPostsUrls = sb.ToString();
                    }
                }
            }

            return posts.OrderByDescending(x => x.CreatedOn).ToArray();
        }

        public async Task UpdateAsync(string id, EditPostViewModel input)
        {
            var post = this.postsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);
            post.Content = input.Content;
            post.ModifiedOn = DateTime.UtcNow;

            await this.postsRepository.SaveChangesAsync();

            if (input.Tags != null)
            {
                var tagsArray = input.Tags.Split(',');

                foreach (var tag in tagsArray)
                {
                    Tag currTag = null;

                    if (this.tagsRepository.All().Any(t => t.Name == tag))
                    {
                        currTag = this.tagsRepository.All().FirstOrDefault(t => t.Name == tag);
                    }
                    else
                    {
                        currTag = new Tag { Name = tag };
                        await this.tagsRepository.AddAsync(currTag);
                        await this.tagsRepository.SaveChangesAsync();
                    }

                    await this.postsTagsRepository.AddAsync(new PostTag { PostId = post.Id, TagId = currTag.Id });
                    await this.postsTagsRepository.SaveChangesAsync();
                }
            }
        }
    }
}
