using System;
using System.Collections.Generic;
using Entities;
using RepositoryInterfaces;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {

        public UserRepository(IUserValidator validator, IEnumerator<int> idGenerator)
        {

        }


        public int Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public List<int> SearchForUsers(Func<User>[] criteria)
        {
            throw new NotImplementedException();
        }
    }
}
