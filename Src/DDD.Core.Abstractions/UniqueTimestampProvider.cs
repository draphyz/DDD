using System;
using Conditions;

namespace DDD
{
    /// <summary>
    /// Provides unique timestamps with a specified resolution.
    /// </summary>
    public class UniqueTimestampProvider : ITimestampProvider
    {

        #region Fields

        private readonly object locker = new object();
        private readonly ITimestampProvider provider;
        private DateTime lastTimestamp = DateTime.MinValue;

        #endregion Fields

        #region Constructors

        public UniqueTimestampProvider(ITimestampProvider provider, TimeSpan resolution)
        {
            Condition.Requires(provider, nameof(provider)).IsNotNull();
            this.provider = provider;
            this.Resolution = resolution;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates the resolution of the provided timestamps.
        /// </summary>
        public TimeSpan Resolution { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Provides a unique timestamp.
        /// </summary>
        public DateTime GetTimestamp()
        {
            var timestamp = this.provider.GetTimestamp();
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