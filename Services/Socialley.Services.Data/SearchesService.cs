using Socialley.Data.Common.Repositories;
using Socialley.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socialley.Services.Data
{
    public class SearchesService : ISearchesService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public SearchesService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public string[] Searches()
        {
            var titles = this.usersRepository
                .All()
                .Select(x => x.UserName)
                .ToArray();

            return titles;
        }
    }
}
