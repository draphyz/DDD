using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a request to a repository has expired.
    /// </summary>
    public class RepositoryTimeoutException : RepositoryException
    {

        #region Constructors

        public RepositoryTimeoutException(Type entityType = null, Exception innerException = null)
            : base(true, DefaultMessage(entityType), entityType, innerException)
        {
        }

        public RepositoryTimeoutException(string message, Type entityType = null, Exception innerException = null)
            : base(true, message, entityType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type entityType = null)
        {
            if (entityType == null)
                return "The request to the repository has expired.";
            return $"The request to the repository of '{entityType.Name}' has expired.";
        }

        #endregion Methods

    }
}
