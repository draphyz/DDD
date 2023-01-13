using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// The base class for all exceptions thrown in the application layer.
    /// </summary>
    public abstract class ApplicationException : TimestampedException
    {

        #region Constructors

        protected ApplicationException(bool isTransient, Exception innerException = null)
            : base(isTransient, DefaultMessage(), innerException)
        {
        }

        protected ApplicationException(bool isTransient, string message, Exception innerException = null)
            : base(isTransient, message, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static string DefaultMessage() => "An error occurred in the application layer.";

        #endregion Methods

    }
}
