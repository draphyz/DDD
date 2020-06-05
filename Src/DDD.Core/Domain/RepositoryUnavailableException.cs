using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a repository is currently unavailable.
    /// </summary>
    public class RepositoryUnavailableException : RepositoryException
    {

        #region Constructors

        public RepositoryUnavailableException(Type entityType = null, Exception innerException = null)
            : base(true, DefaultMessage(entityType), entityType, innerException)
        {
        }

        public RepositoryUnavailableException(string message, Type entityType = null, Exception innerException = null)
            : base(true, message, entityType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type entityType = null)
        {
            if (entityType == null)
                return "The repository is currently unavailable.";
            return $"The repository of '{entityType.Name}' is currently unavailable.";
        }

        #endregion Methods

    }
}
