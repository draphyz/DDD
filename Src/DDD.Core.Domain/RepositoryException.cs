using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown by repositories when a problem occurred while saving or finding domain entities.
    /// </summary>
    public class RepositoryException : Exception
    {

        #region Constructors

        public RepositoryException() 
            : base("A problem occurred while saving or finding domain entities.")
        {
        }

        public RepositoryException(string message) : base(message)
        {
        }

        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RepositoryException(string message, Exception innerException, Type entityType) : base(message, innerException)
        {
            this.EntityType = entityType;
        }

        public RepositoryException(Exception innerException, Type entityType)
            : base($"A problem occurred while saving or finding domain entities of type '{entityType.Name}'.", innerException)
        {
            this.EntityType = entityType;
        }

        #endregion Constructors

        #region Properties

        public Type EntityType { get; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            if (this.EntityType != null)
                s += $"{Environment.NewLine}Entity type: {this.EntityType}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
