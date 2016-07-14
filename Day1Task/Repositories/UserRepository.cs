using System;
using System.Collections.Generic;
using Entities;
using RepositoryInterfaces;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace Repositories
{
    public class UserRepository : IUserRepository, IXmlStorableRepository
    {
        private readonly IUserValidator _validator;
        private readonly IEnumerator<int> _idGenerator;
        private readonly XmlSerializer _serializer;
        private int _generatorMoveCount = 0;     

        // Using of List<> is temporary becose
        // changing user in external code causes changing it in reposity.
        private List<User> _list;

        public UserRepository(IUserValidator validator, IEnumerator<int> idGenerator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException
                    (nameof(validator) + " is null.");
            }
            _validator = validator;

            if (idGenerator == null)
            {
                throw new ArgumentNullException
                    (nameof(idGenerator) + " is null.");
            }
            _idGenerator = idGenerator;

            _list = new List<User>();
            _serializer = new XmlSerializer(typeof(UserRepositoryInfo),
                new[] { typeof(User), typeof(VisaRecord), typeof(List<User>)});
        }

        public int Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException
                    (nameof(user) + " is null.");
            }
            var element = _list.Where((u) => u.Equals(user));
            if (element.Count() != 0)
            {
                throw new UserRepositoryException
                    ("User already added.");
            }
            _validator.Validate(user);
            _idGenerator.MoveNext();
            _generatorMoveCount++;
            user.Id = _idGenerator.Current;
            _list.Add(user);
            return user.Id;
        }

        public void Delete(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException
                    (nameof(user) + " is null.");
            }
            if (!_list.Remove(user))
            {
                throw new UserRepositoryException
                    ("User doesn't exist.");
            }           
        }

        public IQueryable<User> SearchForUsers(params Func<User,bool>[] criterias)
        {
            var result = _list.Where((u) =>
            {
                bool selected = true;
                for (int i = 0; i < criterias.Length; i++)
                {
                    if (criterias[i] == null)
                        throw new ArgumentException("One of criterias is null.");
                    selected &= criterias[i](u);
                }
                return selected;
            });
            return result.AsQueryable();
        }

        public void SaveToXml(Stream writeStream)
        {
            var info = new UserRepositoryInfo()
            {
                Users = _list,
                GeneratorMoveNextCount = _generatorMoveCount,
                GeneratorTypeFullName = _idGenerator.GetType().FullName
            };
            try
            {
                _serializer.Serialize(writeStream, info);
            }
            catch (IOException ex)
            {
                throw new UserRepositoryException
                    ("An IO error occured while saving repository.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new UserRepositoryException
                    ("An Xml serialization error" +
                    "occured while saving repository.", ex);
            }

        }

        public void RestoreFromXml(Stream readStream)
        {
            UserRepositoryInfo info;
            try
            {
                info = _serializer.Deserialize(readStream) as UserRepositoryInfo;
            }
            catch(IOException ex)
            {
                throw new UserRepositoryException
                    ("An IO error occured while restoring repository.",ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new UserRepositoryException
                    ("An Xml serialization error" +
                    "occured while restoring repository.", ex);
            }

            _list = info.Users;
            _generatorMoveCount = info.GeneratorMoveNextCount;
            if (_idGenerator.GetType().FullName != info.GeneratorTypeFullName)
            {
                throw new UserRepositoryException
                    ("Type of idGenerator doesn't mutch the" +
                     " type of generator in saved repository.");
            }
        }
    }
}
