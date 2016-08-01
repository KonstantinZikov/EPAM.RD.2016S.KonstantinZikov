using System;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntitiesTests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Equals_UsersWithDifferentFullNames_False()
        {
            // Arrange
            var user1 = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin"
            };
            var user2 = new User()
            {
                FirstName = "John",
                LastName = "Smith"
            };

            // Act
            bool result = user1.Equals(user2);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_UsersWithDifferentFirstNamesAndSameLastNames_False()
        {
            // Arrange
            var lastName = "Pupkin";
            var user1 = new User()
            {
                FirstName = "Vasya",
                LastName = lastName
            };
            var user2 = new User()
            {
                FirstName = "Leha",
                LastName = lastName
            };

            // Act
            bool result = user1.Equals(user2);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_UsersWithDifferentLastNamesAndSameFirstNames_False()
        {
            // Arrange
            var firstName = "Vasya";
            var user1 = new User()
            {
                FirstName = firstName,
                LastName = "Pupkin"
            };
            var user2 = new User()
            {
                FirstName = firstName,
                LastName = "Pupcov"
            };

            // Act
            bool result = user1.Equals(user2);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equals_UsersWithSameFullNames_True()
        {
            // Arrange
            var firstName = "Vasya";
            var lastname = "Pupkin";
            var user1 = new User()
            {
                FirstName = firstName,
                LastName = lastname
            };
            var user2 = new User()
            {
                FirstName = firstName,
                LastName = lastname
            };

            // Act
            bool result = user1.Equals(user2);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Equals_UsersWithSameFullNamesAndDifferentAnotherData_True()
        {
            // Arrange
            var firstName = "Vasya";
            var lastname = "Pupkin";
            var user1 = new User()
            {
                FirstName = firstName,
                LastName = lastname,
                DateOfBirth = new DateTime(1),
                Gender = Gender.Male,
                PersonalId = "1"
            };           
            var user2 = new User()
            {
                FirstName = firstName,
                LastName = lastname,
                DateOfBirth = new DateTime(2),
                Gender = Gender.Female,
                PersonalId = "2"
            };
            user2.VisaRecords.Add(new VisaRecord());

            // Act
            bool result = user1.Equals(user2);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Equals_UserAndSimpleObject_False()
        {
            // Arrange
            var user = new User();
            var obj = new object();

            // Act
            bool result = user.Equals(obj);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GetHashCode_OneUser_TwoCallsReturnsSameValues()
        {
            // Arrange
            var user = new User();

            // Act
            int hash1 = user.GetHashCode();
            int hash2 = user.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_TwoUsersWithSameFullNames_ReturnsSameValues()
        {
            // Arrange
            var firstName = "Vasya";
            var lastname = "Pupkin";
            var user1 = new User()
            {
                FirstName = firstName,
                LastName = lastname
            };
            var user2 = new User()
            {
                FirstName = firstName,
                LastName = lastname
            };

            // Act
            int hash1 = user1.GetHashCode();
            int hash2 = user2.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void GetHashCode_TwoUsersWithDifferentFullNames_ReturnsDifferentValues()
        {
            // Arrange
            var lastname = "Pupkin";
            var user1 = new User()
            {
                FirstName = "Vasya",
                LastName = lastname
            };
            var user2 = new User()
            {
                FirstName = "Leha",
                LastName = lastname
            };

            // Act
            int hash1 = user1.GetHashCode();
            int hash2 = user2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
