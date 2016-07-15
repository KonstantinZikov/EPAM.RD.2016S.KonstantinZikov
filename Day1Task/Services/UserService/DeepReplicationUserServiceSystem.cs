using ServiceInterfaces;
using System;
using System.Collections.Generic;
using RepositoryInterfaces;

namespace Services
{
    class DeepReplicationUserServiceSystem : BaseReplicationUserServiceSystem
    {
        public DeepReplicationUserServiceSystem(
            IUserRepositoryFactory factory,
            IUserServiceDistributer distributer,
            int slaveCount = 16) : base(distributer)
        {
            if (factory == null)
            {
                throw new ArgumentNullException
                    (nameof(factory) + "is null.");
            }
            if (slaveCount < 0)
            {
                throw new ArgumentOutOfRangeException
                    (nameof(slaveCount) + "must be greater than 0.");
            }

            var slaves = new List<UserService>(slaveCount);
            _distributer.Master = new UserService(factory.GetRepository());
            for (int i = 0; i < slaveCount; i++)
            {
                slaves.Add(new UserService(factory.GetRepository()));
            }
        }
    }
}
