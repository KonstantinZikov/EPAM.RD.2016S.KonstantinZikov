using System;

namespace ServiceInterfaces
{
    public class UserServiceException : Exception
    {
        public UserServiceException() : base()
        {
        }

        public UserServiceException(string message) : base(message)
        {
        }

        public UserServiceException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
