using ServiceInterfaces;
using System;
using System.Collections.Generic;
using RepositoryInterfaces;
using Utils;

namespace Services
{
    // This replication system use one repository for all services.

    [Obsolete("This replicator is obsolete. Use DeepReplicationUserServiceSystem class.")]
    public class ReplicationUserServiceSystem : BaseReplicationUserServiceSystem
    {              
        public ReplicationUserServiceSystem(IUserRepository repository, 
            ILogger logger,
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
            _distributer.Master = new UserService(repository,logger);
            for (int i = 0; i < slaveCount; i++)
            {
                slaves.Add(new UserService(repository,logger));
            }      
        }
    }
}
