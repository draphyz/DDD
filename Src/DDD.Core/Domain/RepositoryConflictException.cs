using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a conflict has been detected while saving a domain entity.
    /// </summary>
    public class RepositoryConflictException : RepositoryException
    {

        #region Constructors

        public RepositoryConflictException(Type entityType = null, Exception innerException = null)
            : base(true, DefaultMessage(entityType), entityType, innerException)
        {
        }

        public RepositoryConflictException(string message, Type entityType = null, Exception innerException = null)
            : base(true, message, entityType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type entityType = null)
        {
            if (entityType == null)
                return "A conflict has been detected while saving a domain entity.";
            return $"A conflict has been detected while saving a domain entity '{entityType.Name}'.";
        }

        #endregion Methods

    }
}
