using System;
using System.Linq;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryInterfaces;
using ServiceInterfaces;

namespace ServicesTests
{
    public abstract class BaseUserServiceTests
    {
        protected abstract IUserService GetService(IUserRepository repository);

        [TestMethod]
        public void Add_TwentyUsers_RepositoryCallAddTwentyTimes()
        {
            // Arrange
            var repo = new PseudoUserRepository();
            var service = GetService(repo);

            // Act
            for (int i = 0; i < 20; i++)
            {
                service.Add(new User());
            }

            // Assert
            Assert.AreEqual(20, repo.AddedCount);
        }

        [TestMethod]
        public void Delete_TwentyUsers_RepositoryCallDeleteTwentyTimes()
        {
            // Arrange
            var repo = new PseudoUserRepository();
            var service = GetService(repo);
            for (int i = 0; i < 20; i++)
            {
                service.Add(null);
            }

            // Act
            for (int i = 0; i < 20; i++)
            {
                service.Delete(null);
            }

            // Assert
            Assert.AreEqual(20, repo.DeletedCount);
        }

        [TestMethod]
        public void Search_SearchTwentyTimes_RepositoryCallSearchTwentyTimes()
        {
            // Arrange
            var repo = new PseudoUserRepository();
            var service = GetService(repo);

            // Act
            for (int i = 0; i < 20; i++)
            {
                service.Search(null);
            }

            // Assert
            Assert.AreEqual(20, repo.SearchedCount);
        }

        [TestMethod]
        public void SaveToXml_XmlStorableRepository_RepositoryCallSaveToXmlMethod()
        {
            // Arrange
            var repo = new PseudoUserRepositoryXml();
            var service = GetService(repo);

            // Act
            service.SaveToXml(null);

            // Assert
            Assert.IsTrue(repo.Saved);
        }

        [TestMethod]
        public void RestoreFromXml_XmlStorableRepository_RepositoryCallRestoreFromXmlMethod()
        {
            // Arrange
            var repo = new PseudoUserRepositoryXml();
            var service = GetService(repo);

            // Act
            service.RestoreFromXml(null);

            // Assert
            Assert.IsTrue(repo.Restored);
        }

        [TestMethod]
        [ExpectedException(typeof(UserServiceException))]
        public void SaveToXml_NotXmlStorableRepository_UserServiceException()
        {
            // Arrange
            var repo = new PseudoUserRepository();
            var service = GetService(repo);

            // Act
            service.SaveToXml(null);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(UserServiceException))]
        public void RestoreFromXml_NotXmlStorableRepository_UserServiceException()
        {
            // Arrange
            var repo = new PseudoUserRepository();
            var service = GetService(repo);

            // Act
            service.RestoreFromXml(null);

            // Assert is handled by exception
        }
    }
}
