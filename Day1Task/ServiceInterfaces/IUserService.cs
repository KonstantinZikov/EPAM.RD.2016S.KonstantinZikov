using Entities;
using System.Collections.Generic;

namespace ServiceInterfaces
{
    interface IUserService
    {
        bool IsMaster { get; }
        int Add(User user);
        List<User> Search();
        void Delete();
    }
}
