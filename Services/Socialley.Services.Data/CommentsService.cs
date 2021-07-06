﻿namespace Socialley.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> repository;

        public CommentsService(IDeletableEntityRepository<Comment> repository)
        {
            this.repository = repository;
        }

        public async Task Create(string postId, string userId, string content, string parentId = null)
        {
            var comment = new Comment
            {
                Content = content,
                PostId = postId,
                ParentId = parentId,
                UserId = userId,
            };

            await this.repository.AddAsync(comment);
            await this.repository.SaveChangesAsync();
        }

        public bool IsInPostId(string commentId, string postId)
        {
            var commentPostId = this.repository.All().Where(x => x.Id == commentId)
               .Select(x => x.PostId).FirstOrDefault();
            return commentPostId == postId;
        }
    }
}
