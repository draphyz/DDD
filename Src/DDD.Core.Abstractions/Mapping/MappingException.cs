using System;

namespace DDD.Mapping
{
    /// <summary>
    /// Exception thrown when an error occurred while mapping (or translating) an input object of one type to an output object of another type.
    /// </summary>
    public class MappingException : Exception
    {

        #region Constructors

        public MappingException(Type sourceType = null, Type destinationType = null, Exception innerException = null)
            : base(DefaultMessage(sourceType, destinationType), innerException)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
        }

        public MappingException(string message, Type sourceType = null, Type destinationType = null, Exception innerException = null)
            : base(message, innerException)
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

        public static string DefaultMessage(Type sourceType = null, Type destinationType = null)
        {
            return $"An error occurred while mapping an input object of {SourceTypeInfo(sourceType)} to an output object of {DestinationTypeInfo(destinationType)}.";
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            if (this.SourceType != null)
                s += $"{Environment.NewLine}SourceType: {this.SourceType}";
            if (this.DestinationType != null)
                s += $"{Environment.NewLine}DestinationType: {this.DestinationType}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        private static string DestinationTypeInfo(Type destinationType)
        {
            if (destinationType == null)
                return "another type";
            return $"the type '{destinationType.Name}'";
        }

        private static string SourceTypeInfo(Type sourceType)
        {
            if (sourceType == null)
                return "one type";
            return $"the type '{sourceType.Name}'";
        }

        #endregion Methods
    }
}
