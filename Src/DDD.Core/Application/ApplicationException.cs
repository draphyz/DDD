using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// The base class for all exceptions thrown in the application layer.
    /// </summary>
    public abstract class ApplicationException : Exception
    {

        #region Constructors

        protected ApplicationException(bool isTransient, Exception innerException = null)
            : base(DefaultMessage(), innerException)
        {
            this.IsTransient = isTransient;
        }

        protected ApplicationException(bool isTransient, string message, Exception innerException = null)
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

        public static string DefaultMessage() => "An error occurred in the application layer.";

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
