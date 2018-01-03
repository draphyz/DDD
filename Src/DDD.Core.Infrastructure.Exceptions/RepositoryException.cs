using System;

namespace DDD.Core.Infrastructure
{
    /// <summary>
    /// Exception thrown by repositories when a problem occurred while saving or finding a domain entity.
    /// </summary>
    public class RepositoryException : Exception
    {

        #region Constructors

        public RepositoryException() 
            : base("A problem occurred while saving or finding a domain entity.")
        {
        }

        public RepositoryException(string message) : base(message)
        {
        }

        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors
    }
}
