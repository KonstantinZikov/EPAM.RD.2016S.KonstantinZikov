using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Entities;
using RepositoryInterfaces;
using Utils;

namespace Repositories
{
    public class UserRepository : MarshalByRefObject, IUserRepository, IStorableRepository
    {
        private readonly IUserValidator _validator;
        private readonly IEnumerator<int> _idGenerator;
        private readonly XmlSerializer _serializer;
        private int _generatorMoveCount = 0;     

        private List<User> _list;

        public UserRepository(IUserValidator validator, IIdGenerator idGenerator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator) + " is null.");
            }

            this._validator = validator;

            if (idGenerator == null)
            {
                throw new ArgumentNullException(nameof(idGenerator) + " is null.");
            }

            this._idGenerator = idGenerator;

            this._list = new List<User>();
            this._serializer = new XmlSerializer(
                typeof(UserRepositoryInfo),
                new[] { typeof(User), typeof(VisaRecord), typeof(List<User>) });
        }

        public int Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user) + " is null.");
            }

            var element = this._list.Where((u) => u.Equals(user));
            if (element.Count() != 0)
            {
                throw new UserRepositoryException("User already added.");
            }

            this._validator.Validate(user);
            this._idGenerator.MoveNext();
            this._generatorMoveCount++;
            user.Id = this._idGenerator.Current;
            this._list.Add(user);
            return user.Id;
        }

        public void Delete(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user) + " is null.");
            }

            if (!this._list.Remove(user))
            {
                throw new UserRepositoryException("User doesn't exist.");
            }           
        }

        public IQueryable<User> Search(params Func<User, bool>[] criterias)
        {
            var result = this._list.Where((u) =>
            {
                bool selected = true;
                for (int i = 0; i < criterias.Length; i++)
                {
                    if (criterias[i] == null)
                    {
                        throw new ArgumentException("One of criterias is null.");
                    }
                        
                    selected &= criterias[i](u);
                }

                return selected;
            });
            return result.AsQueryable();
        }

        public void Save(Stream writeStream)
        {
            var info = new UserRepositoryInfo()
            {
                Users = this._list,
                GeneratorMoveNextCount = this._generatorMoveCount,
                GeneratorTypeFullName = this._idGenerator.GetType().FullName
            };
            try
            {
                Serializer.Serialize(info, writeStream);
            }
            catch (IOException ex)
            {
                throw new UserRepositoryException("An IO error occured while saving repository.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new UserRepositoryException(
                    "A serialization error occured while saving repository.", ex);
            }
        }

        public void Restore(Stream readStream)
        {
            UserRepositoryInfo info;
            try
            {
                info = Serializer.Deserialize(readStream) as UserRepositoryInfo;
            }
            catch (IOException ex)
            {
                throw new UserRepositoryException("An IO error occured while restoring repository.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new UserRepositoryException(
                    "A serialization error occured while restoring repository.", ex);
            }

            this._list = info.Users;
            this._generatorMoveCount = info.GeneratorMoveNextCount;
            if (this._idGenerator.GetType().FullName != info.GeneratorTypeFullName)
            {
                throw new UserRepositoryException(
                    "Type of idGenerator doesn't mutch the" +
                    " type of generator in saved repository.");
            }
        }
    }
}
