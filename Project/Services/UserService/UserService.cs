using ServiceInterfaces;
using System;
using System.Collections.Generic;
using Entities;
using RepositoryInterfaces;
using System.IO;
using System.Linq;
using Utils;
using System.Threading;
using static System.Diagnostics.TraceEventType;

namespace Services
{
    public class UserService : MarshalByRefObject, IUserService
    {
        protected readonly IUserRepository _repository;
        protected readonly StorableRepository _storableRepository;
        protected readonly ILogger _logger;
        protected bool _isXmlStorable;
        protected ReaderWriterLockSlim _lock;

        public int Id { get; private set; }

        public UserService(int id,IUserRepository repository, ILogger logger)
        {
            Id = id;
            if (repository == null)
            {
                throw new ArgumentNullException
                    (nameof(repository) + " is null.");
            }
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            _repository = repository;      
            _storableRepository = repository as StorableRepository;
            if (_storableRepository != null)
            {
                _isXmlStorable = true;
            }

            _logger = logger;
            _logger.Log(Information,$"Service {Id} created successfully.");
        }

        public int Add(User user)
        {
            try
            {
                _lock.EnterWriteLock();
                _logger.Log(Information, $"Add user {user} by service {Id}.");
                return _repository.Add(user);
            }
            catch (UserRepositoryException ex)
            {
                string msg = $"An error occured, while adding the user. Service {Id}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
           

        public void Delete(User user)
        {
            try
            {
                _lock.EnterWriteLock();
                _logger.Log(Information, $"Delete user {user} by service {Id}.");
                _repository.Delete(user);
            }
            catch (UserRepositoryException ex)
            {
                string msg = $"An error occured, while deleting the user. Service {Id}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
            

        public List<User> Search(params Func<User, bool>[] criterias)
        {
            try
            {
                _lock.EnterReadLock();
                _logger.Log(Information, $"Search users by service {Id}.");                
                return _repository.Search(criterias).ToList();
            }
            catch(UserRepositoryException ex)
            {
                string msg = $"An error occured, while searching the users. Service {Id}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
      

        public void Save(Stream writeStream)
        {
            _logger.Log(Information, $"Save to xml service {Id}.");
            if (_isXmlStorable)
            {
                try
                {
                    _lock.EnterReadLock();       
                    _storableRepository.Save(writeStream);
                }
                catch(UserRepositoryException ex)
                {
                    string msg = $"An error occured, while saving to xml. Service {Id}.";
                    _logger.Log(Error, msg);
                    throw new UserServiceException(msg, ex);
                }   
                finally
                {
                    _lock.ExitReadLock();
                }            
            }
            else
            {
                string msg = $"Internal repository doesn't support saving to xml. Service {Id}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }

        public void Restore(Stream readStream)
        {
            _logger.Log(Information, $"Restore from xml service {Id}.");
            if (_isXmlStorable)
            {
                try
                {
                    _lock.EnterWriteLock();
                    _storableRepository.Restore(readStream);
                }
                catch (UserRepositoryException ex)
                {
                    string msg = $"An error occured, while restoring from xml. Service {Id}.";
                    _logger.Log(Error, msg);
                    throw new UserServiceException(msg, ex);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }

            }
            else
            {
                string msg = $"Internal repository doesn't support restoring from xml. Service {Id}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }
    }
}
