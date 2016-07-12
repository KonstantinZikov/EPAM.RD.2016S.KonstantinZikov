using Entities;
using System;
using System.Collections.Generic;

namespace RepositoryInterfaces
{
    interface IUserRepository
    {
        int Add(User user);
        List<int> SearchForUsers(Func<User>[] criteria);
        void Delete(User user);
        void Delete(int id);
    }
}
