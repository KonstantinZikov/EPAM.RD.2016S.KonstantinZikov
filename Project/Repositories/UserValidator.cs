using System;
using System.Text.RegularExpressions;
using Entities;
using RepositoryInterfaces;

namespace Repositories
{
    /// <summary>
    /// Default class for User's validating.
    /// User isn't valid in next cases:
    ///    <para>--User is null.</para> 
    ///    <para>--User's first name is null or empty.</para> 
    ///    <para>--User's last name is null or empty.</para> 
    ///    <para>--User's birth date greater than maxBirthDate.</para> 
    ///    <para>--User's PersonalId is null or doesn't mutch the personalIdRegex.</para> 
    /// </summary>
    public class UserValidator : MarshalByRefObject, IUserValidator
    {
        private readonly Regex _personalIdRegex;
        private readonly DateTime _maxBirthDate;

        /// <summary>
        /// Initialize validator.
        /// </summary>
        /// <param name="maxBirthDate">Max birth date of user. If user's birth date is greater than maxBirthDate,
        /// Validation exception will be occured.</param>
        /// <param name="personalIdRegex">Regex for users property "PersonalId" in string form. If user's PersonalId doesn't mutch it,
        /// Validation exception will be occured. Symbols '^' and '$' before and after regex are unneccessary.
        /// Default value is ".*" (any value of PersonalId is valid).</param>
        public UserValidator(DateTime maxBirthDate, string personalIdRegex = ".*")
        {
            if (string.IsNullOrEmpty(personalIdRegex))
            {
                throw new ArgumentException($"{ nameof(personalIdRegex)} is empty or null.");
            }

            try
            {
                this._personalIdRegex = new Regex("^" + personalIdRegex + "$");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Error while parsing personalIdRegex.", ex);
            }

            this._maxBirthDate = maxBirthDate;
        }

        /// <summary>
        /// Validate selected user. If user is valid, do nothing. Else throws ValidationException.
        /// </summary>
        /// <param name="user">User for validating.</param>
        /// <exception cref="ValidationException">User isn't valid.</exception>
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

            if (user.DateOfBirth >= this._maxBirthDate)
            {
                throw new ValidationException($"User's birth date must be earlier than {this._maxBirthDate}.");
            }

            if (user.PersonalId == null)
            {
                throw new ValidationException("User's personal id is null.");
            }

            if (!this._personalIdRegex.IsMatch(user.PersonalId))
            {
                throw new ValidationException("User's personal id doesn't match selected regex.");
            }
        }
    }
}
