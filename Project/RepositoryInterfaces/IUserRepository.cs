using Entities;
using System;
using System.Linq;

namespace RepositoryInterfaces
{
    public interface IUserRepository
    {
        int Add(User user);
        IQueryable<User> Search(params Func<User,bool>[] criterias);
        void Delete(User user);
    }
}
