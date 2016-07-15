using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServicesTests
{
    //[TestClass]
    public class DeepReplicationUserServiceSystemTests
    {

        PseudoUserRepositoryFactory _factory;

        public DeepReplicationUserServiceSystemTests()
        {
            _factory = new PseudoUserRepositoryFactory();
        }       
    }
}
