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
    public class UserService : MarshalByRefObject, IUserService
    {
        protected readonly IUserRepository _repository;
        protected readonly IStorableRepository _storableRepository;
        protected readonly ILogger _logger;
        protected bool _isXmlStorable;
        protected ReaderWriterLockSlim _lock;

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
                this._isXmlStorable = true;
            }

            this._logger = logger;
            this._logger.Log(Information, $"Service {Id} created successfully.");
        }

        public int Id { get; private set; }

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
      
        public void Save(Stream writeStream)
        {
            this._logger.Log(Information, $"Save to xml service {Id}.");
            if (this._isXmlStorable)
            {
                try
                {
                    this._lock.EnterReadLock();
                    this._storableRepository.Save(writeStream);
                }
                catch (UserRepositoryException ex)
                {
                    string msg = $"An error occured, while saving to xml. Service {Id}.";
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
                string msg = $"Internal repository doesn't support saving to xml. Service {Id}.";
                this._logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }

        public void Restore(Stream readStream)
        {
            this._logger.Log(Information, $"Restore from xml service {Id}.");
            if (this._isXmlStorable)
            {
                try
                {
                    this._lock.EnterWriteLock();
                    this._storableRepository.Restore(readStream);
                }
                catch (UserRepositoryException ex)
                {
                    string msg = $"An error occured, while restoring from xml. Service {Id}.";
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
                string msg = $"Internal repository doesn't support restoring from xml. Service {Id}.";
                this._logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }
    }
}
