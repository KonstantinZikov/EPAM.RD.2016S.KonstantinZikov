using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
