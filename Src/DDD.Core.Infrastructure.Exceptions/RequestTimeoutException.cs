using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown when the client does not receive a timely response from the server..
    /// </summary>
    public class RequestTimeoutException : ClientServerRequestException
    {

        #region Constructors

        public RequestTimeoutException() 
            : base("The client did not receive a timely response from the server.")
        {
        }

        public RequestTimeoutException(string message) : base(message)
        {
        }

        public RequestTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
