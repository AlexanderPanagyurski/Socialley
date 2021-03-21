namespace Socialley.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Socialley.Data.Common.Repositories;
    using Socialley.Data.Models;

    public class FavoritesService : IFavoritesService
    {
        private readonly IDeletableEntityRepository<FavoritePost> favoritePostsRepository;

        public FavoritesService(IDeletableEntityRepository<FavoritePost> favoritePostsRepository)
        {
            this.favoritePostsRepository = favoritePostsRepository;
        }

        public async Task FavorAsync(string postId, string userId)
        {
            var query = this.favoritePostsRepository
                .All()
                .FirstOrDefault(x => x.PostId == postId && x.UserId == userId);

            if (query == null)
            {
                query = new FavoritePost
                {
                    PostId = postId,
                    UserId = userId,
                };
            }

            await this.favoritePostsRepository.AddAsync(query);
            await this.favoritePostsRepository.SaveChangesAsync();
        }

        public int GetCount(string postId)
        {
            var count = this.favoritePostsRepository.All()
                .Where(x => x.PostId == postId).Count();
            return count;
        }
    }
}
