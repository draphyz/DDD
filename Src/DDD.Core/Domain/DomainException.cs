using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// The base class for all exceptions thrown in the domain layer.
    /// </summary>
    public abstract class DomainException : Exception
    {

        #region Constructors

        protected DomainException(bool isTransient, Exception innerException = null) 
            : base(DefaultMessage(), innerException)
        {
            this.IsTransient = isTransient;
        }

        protected DomainException(bool isTransient, string message, Exception innerException = null) 
            : base(message, innerException)
        {
            this.IsTransient = isTransient;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the exception is transient.
        /// </summary>
        public bool IsTransient { get; }

        #endregion Properties

        #region Methods

        public static string DefaultMessage() => "An error occurred in the domain layer.";

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}IsTransient: {this.IsTransient}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods
    }
}
