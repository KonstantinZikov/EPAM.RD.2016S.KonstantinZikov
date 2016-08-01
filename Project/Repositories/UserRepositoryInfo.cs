using System;
using System.Collections.Generic;
using Entities;

namespace Repositories
{
    [Serializable]
    public class UserRepositoryInfo
    {
        public List<User> Users { get; set; }

        public int GeneratorMoveNextCount { get; set; }

        public string GeneratorTypeFullName { get; set; }
    }
}
