using Entities;
using System;
using System.Linq;

namespace RepositoryInterfaces
{
    public interface IUserRepository
    {
        int Add(User user);
        IQueryable<User> SearchForUsers(Func<User,bool>[] criteria);
        void Delete(User user);
    }
}
