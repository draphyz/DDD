using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when a client-server request cannot be processed by the server due to a conflict with other request(s).
    /// </summary>
    public class RequestConflictException : ClientServerRequestException
    {
        #region Constructors

        public RequestConflictException()
            : base("The request could not be processed by the server due to a conflict with other request(s).")
        {
        }

        public RequestConflictException(string message) : base(message)
        {
        }

        public RequestConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors
    }
}
