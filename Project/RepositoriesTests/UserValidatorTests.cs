using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryInterfaces;
using Entities;
using Repositories;
using System.Text.RegularExpressions;

namespace RepositoriesTests
{
    [TestClass]
    public class UserValidatorTests
    {
        private readonly UserValidator validator 
            = new UserValidator(new DateTime(2017,1,1), @"\d+");

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_Null_ValidationException()
        {
            // Arrange is skipped
            // Act
            validator.Validate(null);

            // Assert is handled by exception
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_EmptyUser_ValidationException()
        {
            // Arrange
            var user = new User();

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_UserWithoutFirstName_ValidationException()
        {
            // Arrange
            var user = new User()
            {
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1",
            };

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_UserWithoutLastName_ValidationException()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Vasya",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1",
            };

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_UserWithNullSizedFirstName_ValidationException()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1",
            };

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_UserWithNullSizedLastName_ValidationException()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1",
            };

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_UserWithNullPersonalId_ValidationException()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
            };

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_UserWithPersonalIdNotMatchedToRegex_ValidationException()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "A",
            };

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_UserWithBirthDateGreaterThanMax_ValidationException()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(2020, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1",
            };

            // Act
            validator.Validate(user);

            // Assert is handled by exception
        }

        [TestMethod]
        public void Validate_UserWithCorrectProperties_DoNothing()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Vasya",
                LastName = "Pupkin",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PersonalId = "1",
            };

            // Act
            validator.Validate(user);

            // Assert is skipped
        }
    }
}
