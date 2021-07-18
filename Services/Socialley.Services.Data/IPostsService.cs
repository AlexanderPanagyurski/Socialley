namespace Socialley.Services.Data
{
    using System.Threading.Tasks;

    using Socialley.Web.ViewModels.Posts;

    public interface IPostsService
    {
        Task<string> CreateAsync(PostCreateInputModel input, string userId, string imagePath);

        PostViewModel[] GetPosts(string userId);

        PostViewModel[] GetAllPosts(string userId);

        PostViewModel GetById(string postId, string userId);

        EditPostViewModel EditPost(string id);

        Task UpdateAsync(string id, EditPostViewModel input);

    }
}
