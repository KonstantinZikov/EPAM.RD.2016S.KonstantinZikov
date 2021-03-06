﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositories;
using RepositoryInterfaces;
using Utils;

namespace RepositoriesTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private readonly string _personalIdRegex = @"\d+";
        private readonly DateTime _maxBirthDate = new DateTime(2017, 1, 1);
      
        [TestMethod]
        public void Add_SimpleUser_ReturnsGeneratedId()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var idGenerator = new FibonacciEnumerator();
            idGenerator.MoveNext();

            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };

            // Act
            int result = repository.Add(user);
            int expectedId = idGenerator.Current;

            // Assert
            Assert.AreEqual(expectedId, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_InvalidUser_ValidationException()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user = new User();

            // Act
            repository.Add(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(UserRepositoryException))]
        public void Add_ExistingUser_UserRepositoryException()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };

            // Act
            repository.Add(user);
            repository.Add(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(UserRepositoryException))]
        public void Add_UsersWithSameFullNames_UserRepositoryException()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user1 = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };
            var user2 = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1995, 2, 3),
                Gender = Gender.Male,
                PersonalId = "12"
            };

            // Act
            repository.Add(user1);
            repository.Add(user2);

            // Assert is handled by exception
        }

        [TestMethod]
        public void Add_TwentyDifferentUsers_ReturnsGeneratedId()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var userList = new List<User>();
            for (int i = 0; i < 20; i++)
            {
                userList.Add(new User()
                {
                    FirstName = $"Vasya{i}",
                    LastName = "Pupkin",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = Gender.Male,
                    PersonalId = i.ToString()
                });
            }

            var idGenerator = new FibonacciEnumerator();
            idGenerator.MoveNext();

            // Act
            foreach (var user in userList)
            {
                int resultId = repository.Add(user);
                int expectedId = idGenerator.Current;
                idGenerator.MoveNext();

                // Assert
                Assert.AreEqual(expectedId, resultId);
            }
        }

        [TestMethod]
        public void SearchForUsers_SearchNoExistingUser_EmptyCollection()
        {
            // Arrange 
            var repository = this.CreateRepository();

            // Act
            var result = repository.Search(new Func<User, bool>[] 
            {
                (u) => u.FirstName == "Vasya"
            }).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SearchForUsers_SearchUserByFirstName_SearchedUser()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };
            repository.Add(user);

            // Act
            var result = repository.Search(new Func<User, bool>[] 
            {
                (u) => u.FirstName == "Vasya"
            }).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(user, result[0]);
        }

        [TestMethod]
        public void SearchForUsers_SearchUserByFirstAndLastName_SearchedUser()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user1 = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };
            var user2 = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupcov",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };
            repository.Add(user1);
            repository.Add(user2);

            // Act
            var result = repository.Search(new Func<User, bool>[] 
            {
                (u) => u.FirstName == "Vasya",
                (u) => u.LastName == "Pupkin"
            }).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(user1, result[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(UserRepositoryException))]
        public void Delete_DeleteNotAddedUser_RepositoryException()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };

            // Act
            repository.Delete(user);

            // Assert is hadled by exception
        }

        [TestMethod]
        public void Delete_DeleteAddedUser_UserCannotBeFoundBySearchMethod()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };

            // Act
            repository.Add(user);
            repository.Delete(user);
            var result = repository.Search(new Func<User, bool>[] 
            {
                (u) => u.FirstName == "Vasya"
            }).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Delete_DeleteAddedUser_UserCanBeAddedAgain()
        {
            // Arrange 
            var repository = this.CreateRepository();
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1"
            };

            // Act
            repository.Add(user);
            repository.Delete(user);
            repository.Add(user);
            var result = repository.Search(new Func<User, bool>[]
            {
                (u) => u.FirstName == "Vasya"
            }).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(user, result[0]);
        }

        [TestMethod]
        public void Save_RepositoryWithSomeUsers_RestoresCorrectly()
        {
            // Arrange
            var firstName1 = "Vasya";
            var firstName2 = "Petya";
            using (var stream = new MemoryStream())
            {
                var repository = this.CreateRepository();
                var user1 = new User()
                {
                    FirstName = firstName1,
                    LastName = "Pupkin",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = Gender.Male,
                    PersonalId = "1"
                };
                var user2 = new User()
                {
                    FirstName = firstName2,
                    LastName = "Pupkin",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = Gender.Male,
                    PersonalId = "2"
                };
                repository.Add(user1);
                repository.Add(user2);

                // Act
                repository.Save(stream);
                stream.Position = 0;
                repository.Restore(stream);
                var restored1 = repository.Search((u) => u.FirstName == firstName1).ToList();
                var restored2 = repository.Search((u) => u.FirstName == firstName2).ToList();

                // Assert        
                Assert.AreEqual(1, restored1.Count);   
                Assert.AreEqual(user1, restored1[0]);
                Assert.AreEqual(1, restored2.Count);
                Assert.AreEqual(user2, restored2[0]);
            }           
        }

        private UserRepository CreateRepository()
        {
            var validator = new UserValidator(this._maxBirthDate, this._personalIdRegex);
            var idGenerator = new FibonacciEnumerator();
            return new UserRepository(validator, idGenerator);
        }
    }
}
