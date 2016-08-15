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

    /// <summary>
    /// Simple class for storing User entities. Use IUserValidator object for validating and IIdGenerator for generating id's.
    /// </summary>
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

        /// <summary>
        /// Add user to internal collection. 
        /// User's field "Id" will be ignored and changed to generated value.
        /// </summary>
        /// <param name="user">Adding user.</param>
        /// <returns>Generated id.</returns>
        /// <exception cref="ArgumentNullException">User is null.</exception>
        /// <exception cref="UserRepositoryException">Collection already contains selected user.</exception>
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

        /// <summary>
        /// Search selected user in collection and try to delete it;
        /// </summary>
        /// <param name="user">Deleting user.</param>
        /// <exception cref="ArgumentNullException">User is null.</exception>
        /// <exception cref="UserRepositoryException">Collection doesn't contains user.</exception>
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

        /// <summary>
        /// Search users by selected criterias;
        /// </summary>
        /// <param name="criterias">Array of criterias for search</param>
        /// <exception cref="ArgumentException">One of criterias is null.</exception>
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

        /// <summary>
        /// Serialize storage and write it to selected stream. 
        /// </summary>
        /// <param name="writeStream">Stream to write.</param>
        /// <exception cref="ArgumentException">writeStream isn't writeable.</exception>
        /// <exception cref="UserRepositoryException">Throws when Input/Output or serialization errors are occured.</exception>
        public void Save(Stream writeStream)
        {
            if (writeStream.CanWrite == false)
            {
                throw new ArgumentException($"{nameof(writeStream)} must be writeable.");
            }

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
                throw new UserRepositoryException("An IO error is occured while saving repository.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new UserRepositoryException(
                    "A serialization error is occured while saving repository.", ex);
            }
        }

        /// <summary>
        /// Deserialize storage from the selected stream. 
        /// All data which was added before deserializing will be lost.
        /// </summary>
        /// <param name="readStream">Stream to read.</param>
        /// <exception cref="ArgumentException">readStream isn't readable.</exception>
        /// <exception cref="UserRepositoryException">Throws when Input/Output or serialization errors are occured,
        /// or when type of id generator of deserialized repository doesn't mutch to the current type.</exception>
        public void Restore(Stream readStream)
        {
            if (readStream.CanRead == false)
            {
                throw new ArgumentException($"{nameof(readStream)} must be readable.");
            }

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
