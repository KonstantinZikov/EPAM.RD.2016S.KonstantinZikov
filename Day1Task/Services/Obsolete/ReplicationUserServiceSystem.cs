using ServiceInterfaces;
using System;
using System.Collections.Generic;
using RepositoryInterfaces;

namespace Services
{
    [Obsolete("This replicator is obsolete. Use DeepReplicationUserServiceSystem class.")]
    public class ReplicationUserServiceSystem : BaseReplicationUserServiceSystem
    {              
        public ReplicationUserServiceSystem(IUserRepository repository,
            IUserServiceDistributer distributer,
            int slaveCount = 16) : base(distributer)
        {
            if (repository == null)
            {
                throw new ArgumentNullException
                    (nameof(repository) + "is null.");
            }           
            if (slaveCount < 0)
            {
                throw new ArgumentOutOfRangeException
                    (nameof(slaveCount) + "must be greater than 0.");
            }

            var slaves = new List<UserService>(slaveCount);
            _distributer.Master = new UserService(repository);
            for (int i = 0; i < slaveCount; i++)
            {
                slaves.Add(new UserService(repository));
            }      
        }
    }
}
