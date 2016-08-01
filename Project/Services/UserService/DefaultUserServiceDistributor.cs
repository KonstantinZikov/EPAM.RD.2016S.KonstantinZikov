using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities;
using ServiceInterfaces;
using Utils;

namespace Services
{
    public class DefaultUserServiceDistributor : IUserServiceDistributer
    {
        protected readonly ILogger _logger;
        protected IUserService _master;
        protected List<IUserService> _slaves;
        protected int _slaveCounter;
        protected int _slaveCount;
        
        public DefaultUserServiceDistributor(ILogger logger)
        {
            this.Id = GetHashCode();
            this._logger = logger;
            logger.Log(
                System.Diagnostics.TraceEventType.Information,
                $"UserService distributor {Id} created successfully.");
        }

        public int Id { get; private set; }

        public IUserService Master
        {
            get { return this._master; }
            set { this._master = value; }
        }

        public IEnumerable<IUserService> Slaves
        {
            get
            {
                return this._slaves;
            }

            set
            {
                this._slaves = value.ToList();
                this._slaveCount = this._slaves.Count;
            }
        }

        public virtual int Add(User user)
            => this._master?.Add(user) ?? 0;

        public virtual void Delete(User user)
            => this._master?.Delete(user);

        public virtual List<User> Search(params Func<User, bool>[] criterias)
        {
            if (this._slaveCount == 0)
            {
                return this._master?.Search(criterias) ?? new List<User>();
            }

            var result = this._slaves[this._slaveCounter].Search(criterias);
            this._slaveCounter = (this._slaveCounter + 1) % this._slaves.Count;
            return result;
        }

        public virtual void Restore(Stream readStream)
            => this._master?.Restore(readStream);

        public virtual void Save(Stream writeStream)
            => this._master?.Save(writeStream);       
    }
}
