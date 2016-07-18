﻿using RepositoryInterfaces;
using System;
using Entities;
using System.Text.RegularExpressions;

namespace Repositories
{
    public class UserValidator : IUserValidator
    {
        private readonly Regex _personalIdRegex;
        private readonly DateTime _maxBirthDate;

        public UserValidator(DateTime maxBirthDate, string personalIdRegex = ".*")
        {
            if (string.IsNullOrEmpty(personalIdRegex))
            {
                throw new ArgumentException
                    ($"{ nameof(personalIdRegex)} is empty or null.");
            }
            try
            {
                _personalIdRegex = new Regex("^" + personalIdRegex + "$");
            }
            catch(ArgumentException ex)
            {
                throw new ArgumentException
                    ("Error while parsing personalIdRegex.", ex);
            }
            _maxBirthDate = maxBirthDate;
        }

        public void Validate(User user)
        {
            if (user == null)
            {
                throw new ValidationException("User is null");
            }
            if (string.IsNullOrEmpty(user.FirstName))
            {
                throw new ValidationException("User's first name is empty");
            }
            if (string.IsNullOrEmpty(user.LastName))
            {
                throw new ValidationException("User's last name is empty");
            }
            if (user.DateOfBirth >= _maxBirthDate)
            {
                throw new ValidationException
                    ($"User's birth date must be earlier than {_maxBirthDate}.");
            }
            if (user.PersonalId == null)
            {
                throw new ValidationException("User's personal id is null.");
            }
            if (!_personalIdRegex.IsMatch(user.PersonalId))
            {
                throw new ValidationException
                    ("User's personal id doesn't match selected regex.");
            }

        }
    }
}