using System;

namespace RepositoryInterfaces
{
    class UserRepositoryException : Exception
    {
        public UserRepositoryException() : base() { }
        public UserRepositoryException(string message) : base(message) { }
        public UserRepositoryException(string message, Exception innerException):
            base(message, innerException){ }
    }
}
