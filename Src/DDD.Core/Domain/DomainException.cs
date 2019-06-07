using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// The base class for all exceptions thrown in the domain layer.
    /// </summary>
    public abstract class DomainException : Exception
    {

        #region Constructors

        protected DomainException()
            : base("An error has occurred in the domain layer.")
        {
        }

        protected DomainException(string message) : base(message)
        {
        }

        protected DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
