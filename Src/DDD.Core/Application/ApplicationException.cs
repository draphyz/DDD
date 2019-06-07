using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// The base class for all exceptions thrown in the application layer.
    /// </summary>
    public abstract class ApplicationException : Exception
    {

        #region Constructors

        protected ApplicationException()
            : base("An error has occurred in the application layer.")
        {
        }

        protected ApplicationException(string message) : base(message)
        {
        }

        protected ApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
