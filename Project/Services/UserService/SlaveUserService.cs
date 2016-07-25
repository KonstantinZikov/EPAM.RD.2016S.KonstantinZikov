using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryInterfaces;
using Utils;
using System.Net;

namespace Services
{
    public class SlaveUserService : UserService
    {
        public SlaveUserService(int id, IUserRepository repository, ILogger logger) 
            : base(id, repository, logger)
        {}
    }
}
