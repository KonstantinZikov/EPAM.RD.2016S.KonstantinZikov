using Entities;

namespace Services
{
    class SignalUserServiceDistributor : DefaultUserServiceDistributor
    {
        public override int Add(User user)
        {
            int result = _master.Add(user);
            foreach(var slave in _slaves)
            {
                slave.Add(user);
            }
            return result;
        }
            

        public override void Delete(User user)
        {
            _master.Delete(user);
            foreach (var slave in _slaves)
            {
                slave.Delete(user);
            }
        }

    }
}
