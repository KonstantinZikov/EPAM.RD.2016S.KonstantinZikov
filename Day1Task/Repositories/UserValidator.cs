using RepositoryInterfaces;
using System;
using Entities;
using System.Text.RegularExpressions;

namespace Repositories
{
    public class UserValidator : IUserValidator
    {
        public UserValidator(Regex personalIdRegex, DateTime maxBirthDate)
        {

        }

        public void Validate(User user)
        {
            throw new NotImplementedException();
        }
    }
}
