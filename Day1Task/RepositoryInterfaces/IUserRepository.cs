using Entities;
using System;
using System.Collections.Generic;

namespace RepositoryInterfaces
{
    public interface IUserRepository
    {
        int Add(User user);
        IEnumerable<int> SearchForUsers(Func<User,bool>[] criteria);
        void Delete(User user);
    }
}
