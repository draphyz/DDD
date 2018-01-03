using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when the client-server request is denied by the server.
    /// </summary>
    public class UnauthorizedRequestException : ClientServerRequestException
    {
        #region Constructors

        public UnauthorizedRequestException()
            : base("The request was denied by the server.")
        {
        }

        public UnauthorizedRequestException(string message) : base(message)
        {
        }

        public UnauthorizedRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
