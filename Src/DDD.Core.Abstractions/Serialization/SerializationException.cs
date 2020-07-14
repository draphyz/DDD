using System;

namespace DDD.Serialization
{
    /// <summary>
    /// Exception thrown when an error occurs during serialization or deserialization.
    /// </summary>
    public class SerializationException : Exception
    {

        #region Constructors

        public SerializationException(Type objectType = null, Exception innerException = null)
            : base(DefaultMessage(objectType), innerException)
        {
            this.ObjectType = objectType;
        }

        public SerializationException(string message, Type objectType = null, Exception innerException = null)
            : base(message, innerException)
        {
            this.ObjectType = objectType;
        }

        #endregion Constructors

        #region Properties

        public Type ObjectType { get; }

        #endregion Properties

        #region Methods

        public static string DefaultMessage(Type objectType = null)
        {
            if (objectType == null)
                return "An error occurred while serializing or deserializing an object.";
            return $"An error occurred while serializing or deserializing an object of the type '{objectType.Name}'.";
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            if (this.ObjectType != null)
                s += $"{Environment.NewLine}ObjectType: {this.ObjectType}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
