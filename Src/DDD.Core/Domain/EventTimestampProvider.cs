using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Provides unique timestamps for events.
    /// </summary>
    public static class EventTimestampProvider
    {

        #region Fields

        private readonly static TimestampProvider local = new TimestampProvider(isLocal: true);
        private readonly static TimestampProvider universal = new TimestampProvider(isLocal: false);

        #endregion Fields

        #region Methods

        /// <summary>
        /// Provides a local timestamp with a resolution of one millisecond.
        /// </summary>
        public static DateTime LocalTimestamp() => local.Timestamp();

        /// <summary>
        /// Provides a universal timestamp with a resolution of one millisecond.
        /// </summary>
        public static DateTime UniversalTimestamp() => universal.Timestamp();

        #endregion Methods

    }
}
