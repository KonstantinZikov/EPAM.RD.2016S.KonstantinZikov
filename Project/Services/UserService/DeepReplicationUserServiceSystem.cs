using ServiceInterfaces;
using System.Collections.Generic;
using Utils;
using static System.Diagnostics.TraceEventType;

namespace Services
{
    public class DeepReplicationUserServiceSystem : BaseReplicationUserServiceSystem
    {
        public DeepReplicationUserServiceSystem(
            IUserService master,
            IEnumerable<IUserService> slaves, 
            ILogger logger, 
            IUserServiceDistributer distributer):base(distributer)
        {
            _distributer.Master = master;
            _distributer.Slaves = slaves;
            logger.Log(Information,
                $"Replication service system {Id} created successfully.");
        }
    }
}
