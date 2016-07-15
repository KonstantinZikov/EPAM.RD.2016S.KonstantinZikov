using ServiceInterfaces;
using System;
using System.Collections.Generic;
using Entities;
using System.IO;
using System.Linq;

namespace Services
{
    public class DefaultUserServiceDistributor : IUserServiceDistributer
    {
        protected IUserService _master;
        protected List<IUserService> _slaves;
        protected int slaveCounter;
        protected int slaveCount;

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

        public virtual int Add(User user)
            => _master.Add(user);

        public virtual void Delete(User user)
            => _master.Delete(user);

        public virtual List<User> Search(params Func<User, bool>[] criterias)
        {
            if (slaveCount == 0)
            {
                return _master.Search(criterias);
            }
            var result = _slaves[slaveCounter].Search(criterias);
            slaveCounter = (slaveCounter + 1) % _slaves.Count;
            return result;
        }

        public virtual void RestoreFromXml(Stream readStream)
            => _master.RestoreFromXml(readStream);

        public virtual void SaveToXml(Stream writeStream)
            => _master.SaveToXml(writeStream);       
    }
}
