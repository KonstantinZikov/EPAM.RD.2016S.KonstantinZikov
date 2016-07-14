﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RepositoryInterfaces;
using ServiceInterfaces;
using Services;

namespace ServicesTests
{
    [TestClass]
    public class UserServiceTests : BaseUserServiceTests
    {
        protected override IUserService GetService(IUserRepository repository)
        {
            return new UserService(repository);
        }
    }
}
