using ServiceInterfaces;
using System;
using System.Collections.Generic;
using Entities;
using RepositoryInterfaces;
using System.IO;

namespace Services
{
    class ReplicationUserServiceSystem : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUserServiceDistributer _distributor;
        

        public ReplicationUserServiceSystem(IUserRepository repository,
            IUserServiceDistributer distributor,
            string xmlFileName, int slaveCount = 16)
        {
            // Validate
            if (repository == null)
            {
                throw new ArgumentNullException
                    (nameof(repository) + "is null.");
            }
            if (distributor == null)
            {
                throw new ArgumentNullException
                    (nameof(distributor) + "is null.");
            }
            if (slaveCount < 0)
            {
                throw new ArgumentOutOfRangeException
                    (nameof(slaveCount) + "must be greater than 0.");
            }

            // Initialize
            _repository = repository;
            _distributor = distributor;
         
            var slaves = new List<UserService>(slaveCount);
            _distributor.Master = new UserService(_repository);
            for (int i = 0; i < slaveCount; i++)
            {
                slaves.Add(new UserService(_repository));
            }      
        }

        public int Add(User user)
            => _distributor.Add(user);

        public void Delete(User user)
            => _distributor.Delete(user);

        public List<User> Search(params Func<User, bool>[] criterias)
            => _distributor.Search(criterias);

        public void SaveToXml(Stream writeStream)
            => _distributor.SaveToXml(writeStream);

        public void RestoreFromXml(Stream readStream)
            => _distributor.RestoreFromXml(readStream);
    }
}
