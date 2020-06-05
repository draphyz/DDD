using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when an error occurred while calling a repository.
    /// </summary>
    public class RepositoryException : DomainException
    {

        #region Constructors

        public RepositoryException(bool isTransient, Type entityType = null, Exception innerException = null)
        : base(isTransient, DefaultMessage(entityType), innerException)
        {
            this.EntityType = entityType;
        }

        public RepositoryException(bool isTransient, string message, Type entityType = null, Exception innerException = null)
            : base(isTransient, message, innerException)
        {
            this.EntityType = entityType;
        }

        #endregion Constructors

        #region Properties

        public Type EntityType { get; }

        #endregion Properties

        #region Methods

        public static string DefaultMessage(Type entityType = null)
        {
            if (entityType == null)
                return "An error occurred while saving or finding a domain entity.";
            return $"An error occurred while saving or finding a domain entity '{entityType.Name}'.";
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}IsTransient: {this.IsTransient}";
            if (this.EntityType != null)
                s += $"{Environment.NewLine}EntityType: {this.EntityType}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods
    }
}
