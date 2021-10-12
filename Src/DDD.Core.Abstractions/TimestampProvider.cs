using System;

namespace DDD
{
    /// <summary>
    /// Provides unique timestamps based on the current date and time with a specified resolution in milliseconds.
    /// </summary>
    public class TimestampProvider
    {

        #region Fields

        private readonly object locker = new object();
        private readonly Func<DateTime> now;
        private DateTime lastTimestamp = DateTime.MinValue;

        #endregion Fields

        #region Constructors

        public TimestampProvider(bool isLocal, double resolutionInMs = 1)
        {
            this.IsLocal = isLocal;
            this.ResolutionInMs = resolutionInMs;
            if (isLocal)
                this.now = () => DateTime.Now;
            else
                this.now = () => DateTime.UtcNow;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates whether provided timestamps are local or universal.
        /// </summary>
        public bool IsLocal { get; }

        /// <summary>
        /// Indicates the resolution in milliseconds of provided timestamps.
        /// </summary>
        public double ResolutionInMs { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Provides a timestamp.
        /// </summary>
        public DateTime Timestamp()
        {
            var now = this.now();
            lock (locker)
            {
                if ((now - lastTimestamp).TotalMilliseconds < ResolutionInMs)
                    now = lastTimestamp.AddMilliseconds(ResolutionInMs);
                lastTimestamp = now;
            }
            return now;
        }

        #endregion Methods

    }
}
