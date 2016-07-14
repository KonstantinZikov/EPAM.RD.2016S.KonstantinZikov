using ServiceInterfaces;
using System;
using System.Collections.Generic;
using Entities;
using System.IO;
using System.Linq;

namespace Services
{
    class DefaultUserServiceDistributor : IUserServiceDistributer
    {
        private IUserService _master;
        private List<IUserService> _slaves;
        private int slaveCounter;
        private int slaveCount;

        public IUserService Master
        {
            get { return _master; }
            set { _master = value; }
        }

        public IEnumerable<IUserService> Slaves
        {
            get{ return _slaves; }
            set
            {
                _slaves = value.ToList();
                slaveCount = _slaves.Count;
            }
        }

        public int Add(User user)
            => _master.Add(user);

        public void Delete(User user)
            => _master.Delete(user);

        public List<User> Search(params Func<User, bool>[] criterias)
        {
            if (slaveCount == 0)
            {
                return _master.Search(criterias);
            }
            var result = _slaves[slaveCounter].Search(criterias);
            slaveCounter = (slaveCounter + 1) % _slaves.Count;
            return result;
        }

        public void RestoreFromXml(Stream readStream)
            => _master.RestoreFromXml(readStream);

        public void SaveToXml(Stream writeStream)
            => _master.SaveToXml(writeStream);       
    }
}
