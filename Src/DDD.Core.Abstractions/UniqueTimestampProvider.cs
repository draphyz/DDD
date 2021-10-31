using System;

namespace DDD
{
    /// <summary>
    /// Provides unique timestamps based on the current date and time on this computer with a specified resolution.
    /// </summary>
    public class UniqueTimestampProvider : ITimestampProvider
    {

        #region Fields

        private readonly object locker = new object();
        private readonly Func<DateTime> timestamp;
        private DateTime lastTimestamp = DateTime.MinValue;

        #endregion Fields

        #region Constructors

        public UniqueTimestampProvider(bool isLocal, TimeSpan resolution)
        {
            this.IsLocal = isLocal;
            this.Resolution = resolution;
            if (isLocal)
                this.timestamp = () => DateTime.Now;
            else
                this.timestamp = () => DateTime.UtcNow;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates whether the provided timestamps are local or universal.
        /// </summary>
        public bool IsLocal { get; }

        /// <summary>
        /// Indicates the resolution of the provided timestamps.
        /// </summary>
        public TimeSpan Resolution { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Provides a unique timestamp on this computer.
        /// </summary>
        public DateTime GetTimestamp()
        {
            var timestamp = this.timestamp();
            lock (locker)
            {
                if ((timestamp - lastTimestamp) < Resolution)
                    timestamp = lastTimestamp.Add(Resolution);
                lastTimestamp = timestamp;
            }
            return timestamp;
        }

        #endregion Methods

    }
}