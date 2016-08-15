using System;
using System.Collections.Generic;
using Entities;

namespace Repositories
{
    /// <summary>
    /// Contains all info, needed to save/restore UserRepository to/from stream.
    /// </summary>
    [Serializable]
    public class UserRepositoryInfo
    {
        /// <summary>
        /// Repository users.
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// Count of calls MoveNext() method on IdGenerator. 
        /// </summary>
        public int GeneratorMoveNextCount { get; set; }

        /// <summary>
        /// Full name of IdGenerator type.
        /// </summary>
        public string GeneratorTypeFullName { get; set; }
    }
}
