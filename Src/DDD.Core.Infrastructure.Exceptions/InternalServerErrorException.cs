using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when a generic error occurs on the server.
    /// </summary>
    public class InternalServerErrorException : ClientServerRequestException
    {
        #region Constructors

        public InternalServerErrorException() 
            : base($"A generic error has occurred on the server.")
        {
        }

        public InternalServerErrorException(string message) : base(message)
        {
        }

        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
