using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown by repositories when a concurrency conflict occurred while saving a domain entity.
    /// </summary>
    public class RepositoryConcurrencyException : RepositoryException
    {
        #region Constructors

        public RepositoryConcurrencyException() 
            : base("A concurrency conflict occurred while saving a domain entity.")
        {
        }

        public RepositoryConcurrencyException(string message) : base(message)
        {
        }

        public RepositoryConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors
    }
}
