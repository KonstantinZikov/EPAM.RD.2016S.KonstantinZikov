using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Entities;
using RepositoryInterfaces;
using ServiceInterfaces;
using Utils;

using static System.Diagnostics.TraceEventType;

namespace Services
{

    /// <summary>
    /// Base service, which contains effective realization of IUserService's methods.
    /// </summary>
    public class UserService : MarshalByRefObject, IUserService
    {
        protected readonly IUserRepository _repository;
        protected readonly IStorableRepository _storableRepository;
        protected readonly ILogger _logger;
        protected bool _isStorable;
        protected ReaderWriterLockSlim _lock;

        /// <summary>
        /// Create new instance of UserService with selected service id.
        /// </summary>
        /// <param name="id">Service's id.</param>
        /// <param name="repository">Repository as a lower data layer.</param>
        /// <param name="logger"></param>
        public UserService(int id, IUserRepository repository, ILogger logger)
        {
            this.Id = id;
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository) + " is null.");
            }

            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            this._repository = repository;
            this._storableRepository = repository as IStorableRepository;
            if (this._storableRepository != null)
            {
                this._isStorable = true;
            }

            this._logger = logger;
            this._logger.Log(Information, $"Service {Id} created successfully.");
        }

        /// <summary>
        /// Service's id. Id is used for logging servifce actions. 
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Add selected user to internal repository.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Add(User user)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._logger.Log(Information, $"Add user {user} by service {Id}.");
                return this._repository.Add(user);
            }
            catch (UserRepositoryException ex)
            {
                string msg = $"An error occured, while adding the user. Service {Id}.";
                this._logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }
           
        /// <summary>
        /// Delete selected user from internal repository.
        /// </summary>
        /// <param name="user"></param>
        public void Delete(User user)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._logger.Log(Information, $"Delete user {user} by service {Id}.");
                this._repository.Delete(user);
            }
            catch (UserRepositoryException ex)
            {
                string msg = $"An error occured, while deleting the user. Service {Id}.";
                this._logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }
            
        /// <summary>
        /// Search users which match selected criterias.
        /// </summary>
        /// <param name="criterias"></param>
        /// <returns>List of found users.</returns>
        public List<User> Search(params Func<User, bool>[] criterias)
        {
            try
            {
                this._lock.EnterReadLock();
                this._logger.Log(Information, $"Search users by service {Id}.");                
                return this._repository.Search(criterias).ToList();
            }
            catch (UserRepositoryException ex)
            {
                string msg = $"An error occured, while searching the users. Service {Id}.";
                this._logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Try to write internal repository to stream. 
        /// If repository isn't storable, throws UserServiceException.
        /// </summary>
        /// <param name="writeStream"></param>
        public void Save(Stream writeStream)
        {
            this._logger.Log(Information, $"Save service {Id}.");
            if (this._isStorable)
            {
                try
                {
                    this._lock.EnterReadLock();
                    this._storableRepository.Save(writeStream);
                }
                catch (UserRepositoryException ex)
                {
                    string msg = $"An error occured, while saving. Service {Id}.";
                    this._logger.Log(Error, msg);
                    throw new UserServiceException(msg, ex);
                }   
                finally
                {
                    this._lock.ExitReadLock();
                }            
            }
            else
            {
                string msg = $"Internal repository doesn't support saving. Service {Id}.";
                this._logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }

        /// <summary>
        /// Try to restore internal repository from stream. 
        /// If repository isn't storable, throws UserServiceException.
        /// </summary>
        /// <param name="writeStream"></param>
        public void Restore(Stream readStream)
        {
            this._logger.Log(Information, $"Restore service {Id}.");
            if (this._isStorable)
            {
                try
                {
                    this._lock.EnterWriteLock();
                    this._storableRepository.Restore(readStream);
                }
                catch (UserRepositoryException ex)
                {
                    string msg = $"An error occured, while restoring. Service {Id}.";
                    this._logger.Log(Error, msg);
                    throw new UserServiceException(msg, ex);
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
            else
            {
                string msg = $"Internal repository doesn't support restoring. Service {Id}.";
                this._logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }
    }
}
