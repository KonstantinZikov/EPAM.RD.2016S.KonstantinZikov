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
                $"Distributer send add signal to master service {_master.Id}.");
            return _master.Add(user);
        }
            

        public override void Delete(User user)
        {
            _logger.Log(Information,
                $"Distributer send delete signal to master service {_master.Id}.");
            _master.Delete(user);
        }

    }
}
