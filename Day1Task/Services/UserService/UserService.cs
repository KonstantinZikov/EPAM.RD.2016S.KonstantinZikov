using ServiceInterfaces;
using System;
using System.Collections.Generic;
using Entities;
using RepositoryInterfaces;
using System.IO;
using System.Linq;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IXmlStorableRepository _storableRepository;
        private bool _isXmlStorable;

        public UserService(IUserRepository repository)
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
        }

        public int Add(User user)
        {
            try
            {
                return _repository.Add(user);
            }
            catch (UserRepositoryException ex)
            {
                throw new UserServiceException
                    ("An error occured, while adding the user.", ex);
            }
        }
           

        public void Delete(User user)
        {
            try
            {
                _repository.Delete(user);
            }
            catch (UserRepositoryException ex)
            {
                throw new UserServiceException
                    ("An error occured, while deleting the user.", ex);
            }
        }
            

        public List<User> Search(params Func<User, bool>[] criterias)
        {
            try
            {
                return _repository.Search(criterias).ToList();
            }
            catch(UserRepositoryException ex)
            {
                throw new UserServiceException
                   ("An error occured, while searching the users.", ex);
            }
        }
      

        public void SaveToXml(Stream writeStream)
        {
            if (_isXmlStorable)
            {
                try
                {
                    _storableRepository.SaveToXml(writeStream);
                }
                catch(UserRepositoryException ex)
                {
                    throw new UserServiceException
                        ("An error occured, while saving to xml.", ex);
                }               
            }
            else
            {
                throw new UserServiceException
                    ("Internal repository doesn't support saving to xml.");
            }
        }

        public void RestoreFromXml(Stream readStream)
        {
            if (_isXmlStorable)
            {
                try
                {
                    _storableRepository.RestoreFromXml(readStream);
                }
                catch (UserRepositoryException ex)
                {
                    throw new UserServiceException
                        ("An error occured, while restoring from xml.", ex);
                }

            }
            else
            {
                throw new UserServiceException
                    ("Internal repository doesn't support restoring from xml.");
            }
        }
    }
}
