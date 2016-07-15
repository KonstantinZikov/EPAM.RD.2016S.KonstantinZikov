using ServiceInterfaces;
using System;
using System.Collections.Generic;
using Entities;
using RepositoryInterfaces;
using System.IO;
using System.Linq;
using Utils;
using static System.Diagnostics.TraceEventType;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IXmlStorableRepository _storableRepository;
        private readonly ILogger _logger;
        private bool _isXmlStorable;
       

        public UserService(IUserRepository repository, ILogger logger)
        {
            if (repository == null)
            {
                throw new ArgumentNullException
                    (nameof(repository) + " is null.");
            }
            _repository = repository;      
            _storableRepository = repository as IXmlStorableRepository;
            if (_storableRepository != null)
            {
                _isXmlStorable = true;
            }

            _logger = logger;
            _logger.Log(Information,$"Service {GetHashCode()} created successfully.");
        }

        public int Add(User user)
        {
            try
            {
                _logger.Log(Information, $"Add user {user} by service {GetHashCode()}.");
                return _repository.Add(user);
            }
            catch (UserRepositoryException ex)
            {
                string msg = $"An error occured, while adding the user. Service {GetHashCode()}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
        }
           

        public void Delete(User user)
        {
            try
            {
                _logger.Log(Information, $"Delete user {user} by service {GetHashCode()}.");
                _repository.Delete(user);
            }
            catch (UserRepositoryException ex)
            {
                string msg = $"An error occured, while deleting the user. Service {GetHashCode()}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
        }
            

        public List<User> Search(params Func<User, bool>[] criterias)
        {
            try
            {
                _logger.Log(Information, $"Search users by service {GetHashCode()}.");
                return _repository.Search(criterias).ToList();
            }
            catch(UserRepositoryException ex)
            {
                string msg = $"An error occured, while searching the users. Service {GetHashCode()}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg, ex);
            }
        }
      

        public void SaveToXml(Stream writeStream)
        {
            _logger.Log(Information, $"Save to xml service {GetHashCode()}.");
            if (_isXmlStorable)
            {
                try
                {             
                    _storableRepository.SaveToXml(writeStream);
                }
                catch(UserRepositoryException ex)
                {
                    string msg = $"An error occured, while saving to xml. Service {GetHashCode()}.";
                    _logger.Log(Error, msg);
                    throw new UserServiceException(msg, ex);
                }               
            }
            else
            {
                string msg = $"Internal repository doesn't support saving to xml. Service {GetHashCode()}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }

        public void RestoreFromXml(Stream readStream)
        {
            _logger.Log(Information, $"Restore from xml service {GetHashCode()}.");
            if (_isXmlStorable)
            {
                try
                {
                    _storableRepository.RestoreFromXml(readStream);
                }
                catch (UserRepositoryException ex)
                {
                    string msg = $"An error occured, while restoring from xml. Service {GetHashCode()}.";
                    _logger.Log(Error, msg);
                    throw new UserServiceException(msg, ex);
                }

            }
            else
            {
                string msg = $"Internal repository doesn't support restoring from xml. Service {GetHashCode()}.";
                _logger.Log(Error, msg);
                throw new UserServiceException(msg);
            }
        }
    }
}
