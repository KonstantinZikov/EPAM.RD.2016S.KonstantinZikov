using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using RepositoryInterfaces;

namespace ServicesTests
{
    public class PseudoUserRepository : IUserRepository
    {
        public int AddedCount { get; set; }

        public int DeletedCount { get; set; }

        public int SearchedCount { get; set; }

        public int Add(User user)
        {
            this.AddedCount++;
            return 0;
        }

        public void Delete(User user)
        {
            this.DeletedCount++;
        }

        public IQueryable<User> Search(params Func<User, bool>[] criterias)
        {
            this.SearchedCount++;
            return new List<User>().AsQueryable();
        }
    }
}
