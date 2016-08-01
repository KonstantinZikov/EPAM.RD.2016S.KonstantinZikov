using System.Collections.Generic;
using System.ServiceModel;
using ServiceInterfaces;
using Utils;
using static System.Diagnostics.TraceEventType;

namespace Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class DeepReplicationUserServiceSystem : BaseReplicationUserServiceSystem
    {
        public DeepReplicationUserServiceSystem(
            IUserService master,
            IEnumerable<IUserService> slaves, 
            ILogger logger, 
            IUserServiceDistributer distributer) : base(distributer)
        {
            Distributer.Master = master;
            Distributer.Slaves = slaves;
            logger.Log(
                Information,
                $"Replication service system {Id} created successfully.");
        }
    }
}
