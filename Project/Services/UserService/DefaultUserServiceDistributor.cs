using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities;
using ServiceInterfaces;
using Utils;
using static System.Diagnostics.TraceEventType;

namespace Services
{
    /// <summary>
    /// Distribute Add/Delete/Search operations between masters and slaves.
    /// If Master or slaves aren't selected, operations are suppressed.
    /// </summary>
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
                if (value == null)
                {
                    this._slaves = new List<IUserService>();
                    this._slaveCount = 0;
                }
                else
                {
                    this._slaves = value.ToList();
                    this._slaveCount = this._slaves.Count;
                }               
            }
        }

        /// <summary>
        /// Call master's add method. If master isn't selected, operation is suppressed.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual int Add(User user)
        {
            _logger.Log(
                Information,
                $"Distributer send add signal to master service {_master?.Id ?? 0}.");
            return _master?.Add(user) ?? 0;
        }

        /// <summary>
        /// Call master's delete method. If master isn't selected, operation is suppressed.
        /// </summary>
        /// <param name="user"></param>
        public virtual void Delete(User user)
        {
            _logger.Log(
                Information,
                $"Distributer send delete signal to master service {_master?.Id ?? 0}.");
            _master?.Delete(user);
        }

        /// <summary>
        /// Distribute search operation between slaves. If slaves aren't selected, operation
        /// will called on master. In this case, if master isn't selected, operation is suppressed.
        /// </summary>
        /// <param name="criterias"></param>
        /// <returns></returns>
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
