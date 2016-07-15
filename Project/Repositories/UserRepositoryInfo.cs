using Entities;
using System;
using System.Collections.Generic;

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
