using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Socialley.Services.Data
{
    public interface ICommentsService
    {
        Task Create(string postId, string userId, string content, string parentId = null);

        bool IsInPostId(string commentId, string postId);
    }
}
