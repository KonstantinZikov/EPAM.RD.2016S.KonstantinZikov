using System.Collections.Generic;

namespace ServiceInterfaces
{
    public interface IUserServiceDistributer : IUserService
    {
        IUserService Master { get; set; }
        IEnumerable<IUserService> Slaves { get; set; }
    }
}
