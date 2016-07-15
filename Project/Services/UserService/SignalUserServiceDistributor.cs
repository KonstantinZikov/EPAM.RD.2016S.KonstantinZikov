using Entities;
using Utils;
using static System.Diagnostics.TraceEventType;
namespace Services
{
    public class SignalUserServiceDistributor : DefaultUserServiceDistributor
    {
        public SignalUserServiceDistributor(ILogger logger) : base(logger) { }

        public override int Add(User user)
        {
            _logger.Log(Information, 
                $"Distributer send add signal to master service {_master.GetHashCode()}.");
            int result = _master.Add(user);
            foreach(var slave in _slaves)
            {
                _logger.Log(Information,
                $"Distributer send add signal to slave service {slave.GetHashCode()}.");
                slave.Add(user);
            }
            return result;
        }
            

        public override void Delete(User user)
        {
            _logger.Log(Information,
                $"Distributer send delete signal to master service {_master.GetHashCode()}.");
            _master.Delete(user);
            foreach (var slave in _slaves)
            {
                _logger.Log(Information,
                $"Distributer send delete signal to slave service {slave.GetHashCode()}.");
                slave.Delete(user);
            }
        }

    }
}
