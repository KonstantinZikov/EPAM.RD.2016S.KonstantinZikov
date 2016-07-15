using ServiceInterfaces;
using System;
using System.Collections.Generic;
using RepositoryInterfaces;
using Utils;
using static System.Diagnostics.TraceEventType;

namespace Services
{
    public class DeepReplicationUserServiceSystem : BaseReplicationUserServiceSystem
    {
        public DeepReplicationUserServiceSystem(
            IUserRepositoryFactory factory,
            ILogger logger,
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
            _distributer.Master = new UserService(factory.CreateRepository(),logger);
            logger.Log(Information, $"UserService { _distributer.Master.GetHashCode()} is MASTER.");
            for (int i = 0; i < slaveCount; i++)
            {
                var slave = new UserService(factory.CreateRepository(), logger);
                slaves.Add(slave);
                logger.Log(Information, $"UserService {slave.GetHashCode()} is SLAVE.");
            }
            _distributer.Slaves = slaves;

            logger.Log(Information,
                $"Replication service system {GetHashCode()} created successfully.");
        }
    }
}
