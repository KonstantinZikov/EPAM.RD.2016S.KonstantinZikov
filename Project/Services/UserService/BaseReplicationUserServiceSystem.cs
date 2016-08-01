using System;
using System.Collections.Generic;
using System.IO;
using Entities;
using ServiceInterfaces;

namespace Services
{
    public abstract class BaseReplicationUserServiceSystem : IWcfUserService
    {         
        public BaseReplicationUserServiceSystem(IUserServiceDistributer distributer)
        {
            this.Id = GetHashCode();
            if (distributer == null)
            {
                throw new ArgumentNullException(nameof(distributer) + "is null.");
            }

            this.Distributer = distributer;
        }

        public int Id { get; private set; }

        protected IUserServiceDistributer Distributer { get; set; }

        public int Add(User user)
            => this.Distributer.Add(user);

        public void Delete(User user)
            => this.Distributer.Delete(user);

        public List<User> Search(params Func<User, bool>[] criterias)
            => this.Distributer.Search(criterias);

        public void Save(Stream writeStream)
            => this.Distributer.Save(writeStream);

        public void Restore(Stream readStream)
            => this.Distributer.Restore(readStream);

        public IEnumerable<User> SearchById(int id)
            => this.Distributer.Search(new Func<User, bool>[] 
            {
                (u) => u.Id == id
            });

        public IEnumerable<User> SearchByFirstName(string name)
            => this.Distributer.Search(new Func<User, bool>[] 
            {
                (u) => u.FirstName == name
            });
        
        public IEnumerable<User> SearchByLastName(string name)
            => this.Distributer.Search(new Func<User, bool>[] 
            {
                (u) => u.LastName == name
            });

        public IEnumerable<User> SearchByGender(Gender gender)
            => this.Distributer.Search(new Func<User, bool>[] 
            {
                (u) => u.Gender == gender
            });

        public IEnumerable<User> SearchByDateOfBirth(DateTime date)
           => this.Distributer.Search(new Func<User, bool>[] 
           {
               (u) => u.DateOfBirth == date
           });
    }
}
