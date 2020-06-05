using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a request to a repository was denied.
    /// </summary>
    public class RepositoryUnauthorizedException : RepositoryException
    {

        #region Constructors

        public RepositoryUnauthorizedException(Type entityType = null, Exception innerException = null)
            : base(false, DefaultMessage(entityType), entityType, innerException)
        {
        }

        public RepositoryUnauthorizedException(string message, Type entityType = null, Exception innerException = null)
            : base(false, message, entityType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type entityType = null)
        {
            if (entityType == null)
                return "The request to the repository was denied.";
            return $"The request to the repository of '{entityType.Name}' was denied.";
        }

        #endregion Methods

    }
}
