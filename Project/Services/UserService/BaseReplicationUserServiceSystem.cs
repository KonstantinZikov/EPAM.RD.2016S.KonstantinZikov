using Entities;
using ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Services
{
    public abstract class BaseReplicationUserServiceSystem : IUserService
    {
        protected IUserServiceDistributer _distributer;

        public BaseReplicationUserServiceSystem(IUserServiceDistributer distributer)
        {
            if (distributer == null)
            {
                throw new ArgumentNullException
                    (nameof(distributer) + "is null.");
            }
            _distributer = distributer;
        }

        public int Add(User user)
            => _distributer.Add(user);

        public void Delete(User user)
            => _distributer.Delete(user);

        public List<User> Search(params Func<User, bool>[] criterias)
            => _distributer.Search(criterias);

        public void SaveToXml(Stream writeStream)
            => _distributer.SaveToXml(writeStream);

        public void RestoreFromXml(Stream readStream)
            => _distributer.RestoreFromXml(readStream);
    }
}
