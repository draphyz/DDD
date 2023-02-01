using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// The base class for all exceptions thrown in the domain layer.
    /// </summary>
    public abstract class DomainException : TimestampedException
    {

        #region Constructors

        protected DomainException(bool isTransient, Exception innerException = null) 
            : base(isTransient, DefaultMessage(), innerException)
        {
        }

        protected DomainException(bool isTransient, string message, Exception innerException = null) 
            : base(isTransient, message, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static string DefaultMessage() => "An error occurred in the domain layer.";

        #endregion Methods
    }
}
