namespace Socialley.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Socialley.Web.ViewModels.Tags;

    public interface ITagsService
    {
        Task<ICollection<TagPostsViewModel>> GetTagsPostsAsync(string tag);
    }
}
