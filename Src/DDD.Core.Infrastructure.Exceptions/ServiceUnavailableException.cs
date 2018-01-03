using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when the server or the requested service is currently unavailable.
    /// </summary>
    public class ServiceUnavailableException : ClientServerRequestException
    {
        #region Constructors

        public ServiceUnavailableException() 
            : base("The server or requested service is currently unavailable.")
        {
        }

        public ServiceUnavailableException(string message) : base(message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
