using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryInterfaces;
using ServiceInterfaces;
using Services;

namespace ServicesTests
{
    [TestClass]
    public class ReplicationUserServiceSystemTests : BaseUserServiceTests
    {
        protected override IUserService GetService(IUserRepository repository)
        {
            var distributor = new DefaultUserServiceDistributor();
            return new ReplicationUserServiceSystem(repository,distributor,"default");
        }
    }
}
