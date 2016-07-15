using RepositoryInterfaces;
using System;
using System.Collections.Generic;

namespace ServicesTests
{
    public class PseudoUserRepositoryFactory : IUserRepositoryFactory
    {
        public IEnumerator<IUserRepository> Repositories { get; set; }

        public IUserRepository GetRepository()
        {
            if (Repositories.MoveNext())
            {
                return Repositories.Current;
            }
            throw new InvalidOperationException("Repository enumerator ends.");
            
        }
    }
}
