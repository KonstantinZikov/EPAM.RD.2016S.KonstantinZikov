//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using RepositoryInterfaces;
//using ServiceInterfaces;
//using Services;

//namespace ServicesTests
//{
//    public class ReplicationUserServiceSystemTests : BaseUserServiceTests
//    {
//        protected override IUserService GetService(IUserRepository repository)
//        {
//            var distributor = new DefaultUserServiceDistributor();
//            return new ReplicationUserServiceSystem(repository, distributor);
//        }
//    }
//}

// Here is test for the obsolete class