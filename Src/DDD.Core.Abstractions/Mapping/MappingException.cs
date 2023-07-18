using System;

namespace DDD.Mapping
{
    /// <summary>
    /// Exception thrown by mappers or translators when a problem occurred while mapping objects.
    /// </summary>
    public class MappingException : Exception
    {

        #region Constructors

        public MappingException()
            : base("A problem occurred while mapping objects.")
        {
        }

        public MappingException(string message) : base(message)
        {
        }

        public MappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MappingException(string message, Exception innerException, Type sourceType, Type destinationType) : base(message, innerException)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
        }

        public MappingException(Exception innerException, Type sourceType, Type destinationType)
            : base($"A problem occurred while mapping one object of type '{sourceType?.Name}' to another object of type '{destinationType?.Name}'.", innerException)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
        }

        #endregion Constructors

        #region Properties

        public Type DestinationType { get; }

        public Type SourceType { get; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            if (this.SourceType != null)
                s += $"{Environment.NewLine}{nameof(SourceType)}: {this.SourceType}";
            if (this.DestinationType != null)
                s += $"{Environment.NewLine}{nameof(DestinationType)}: {this.DestinationType}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
