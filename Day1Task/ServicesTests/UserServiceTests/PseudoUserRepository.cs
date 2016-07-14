using Entities;
using RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicesTests
{
    class PseudoUserRepository : IUserRepository
    {
        public int AddedCount { get; set; }
        public int DeletedCount { get; set; }
        public int SearchedCount { get; set; }

        public int Add(User user)
        {
            AddedCount++;
            return 0;
        }

        public void Delete(User user)
        {
            DeletedCount++;
        }

        public IQueryable<User> Search(params Func<User, bool>[] criterias)
        {
            SearchedCount++;
            return new List<User>().AsQueryable();
        }
    }
}
