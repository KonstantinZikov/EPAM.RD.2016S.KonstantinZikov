using System;

namespace Utils
{
    public class LoggerException : Exception
    {
        public LoggerException() : base() { }
        public LoggerException(string message) : base(message) { }
        public LoggerException(string message, Exception innerException):
            base(message, innerException){ }
    }
}
