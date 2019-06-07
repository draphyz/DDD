using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// The base class for all exceptions thrown in the infrastructure layer.
    /// </summary>
    public abstract class InfrastructureException : Exception
    {

        #region Constructors

        protected InfrastructureException()
            : base("An error has occurred in the infrastructure layer.")
        {
        }

        protected InfrastructureException(string message) : base(message)
        {
        }

        protected InfrastructureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors

    }
}
