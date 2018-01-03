using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when a client-server request cannot be processed by the server due to an apparent client error.
    /// </summary>
    public class BadRequestException : ClientServerRequestException
    {
        #region Constructors

        public BadRequestException()
            : base("The request could not be processed by the server due to an apparent client error.")
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors
    }
}
