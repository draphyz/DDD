using System;
using System.Runtime.Serialization;

namespace DDD
{
    /// <summary>
    /// An exception that records the timestamp of when it was thrown and information about its recoverability.
    /// </summary>
    public abstract class TimestampedException : Exception
    {

        #region Constructors

        protected TimestampedException(bool isTransient, string message, Exception innerException = null)
            : base(message, innerException)
        {
            this.Timestamp = SystemTime.Local();
            this.IsTransient = isTransient;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the exception is transient.
        /// </summary>
        public bool IsTransient { get; }

        /// <summary>
        /// Gets the time at which the exception occurred.
        /// </summary>
        public DateTime Timestamp { get; }

        #endregion Properties

        #region Methods

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Timestamp", this.Timestamp.ToString());
            info.AddValue("IsTransient", this.IsTransient);
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}{nameof(Timestamp)}: {this.Timestamp}";
            s += $"{Environment.NewLine}{nameof(IsTransient)}: {this.IsTransient}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
