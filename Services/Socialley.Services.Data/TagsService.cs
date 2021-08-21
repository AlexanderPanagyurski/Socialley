namespace Socialley.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;
    using Socialley.Web.ViewModels.Tags;

    public class TagsService : ITagsService
    {
        private readonly IDeletableEntityRepository<PostTag> tagsRepository;

        public TagsService(IDeletableEntityRepository<PostTag> tagsRepository)
        {
            this.tagsRepository = tagsRepository;
        }

        public async Task<ICollection<TagPostsViewModel>> GetTagsPostsAsync(string tag)
        {
            var tagPosts = await this.tagsRepository
                .All()
                .Include(p => p.Post)
                .ThenInclude(p => p.FavoritePosts)
                .ThenInclude(p => p.Post.ImagePosts)
                .Where(t => t.Tag.Name == tag)
                .Select(t => new TagPostsViewModel
                {
                    PostsId = t.PostId,
                    PostLikes = t.Post.FavoritePosts.Count(x => x.PostId == t.PostId),
                    ImageUrl = "/images/posts/" + t.Post.ImagePosts.FirstOrDefault(x => x.PostId == t.PostId).Id + "." + t.Post.ImagePosts.FirstOrDefault(x => x.PostId == t.PostId).Extension,
                })
                .ToArrayAsync();

            return tagPosts;
        }
    }
}
