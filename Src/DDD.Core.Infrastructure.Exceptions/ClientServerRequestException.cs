using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when a generic error occurs during the processing of a client-server request.
    /// </summary>
    public class ClientServerRequestException : Exception
    {

        #region Constructors

        public ClientServerRequestException() 
            : base("A generic error occurred during the processing of a client-server request.")
        {
        }

        public ClientServerRequestException(string message) : base(message)
        {
        }

        public ClientServerRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
