using System;

namespace DDD
{
    /// <summary>
    /// This class must be used in the production code instead of the standard built-in DateTime to provide local and universal system times.
    /// It allows changing and resetting the current time in unit tests.
    /// </summary>
    public static class SystemTime
    {

        #region Fields

        private static ITimestampProvider localProvider;
        private static ITimestampProvider universalProvider;

        #endregion Fields

        #region Constructors

        static SystemTime()
        {
            Reset();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the current date and time on this computer.
        /// </summary>
        public static DateTime Local() => localProvider.GetTimestamp();

        /// <summary>
        /// Resets the default timestamp providers (for unit testing).
        /// </summary>
        public static void Reset()
        {
            localProvider = DelegatingTimestampProvider.CreateLocal();
            universalProvider = DelegatingTimestampProvider.CreateUniversal();
        }

        /// <summary>
        /// Replaces the default local timestamp provider (for unit testing).
        /// </summary>
        public static void SetLocalProvider(ITimestampProvider provider)
        {
            localProvider = provider;
        }

        /// <summary>
        /// Replaces the default local timestamp provider (for unit testing).
        /// </summary>
        public static void SetLocalProvider(Func<DateTime> timestamp)
        {
            localProvider = new DelegatingTimestampProvider(timestamp);
        }

        /// <summary>
        /// Replaces the default universal timestamp provider (for unit testing).
        /// </summary>
        public static void SetUniversalProvider(ITimestampProvider provider)
        {
            universalProvider = provider;
        }

        /// <summary>
        /// Replaces the default universal timestamp provider (for unit testing).
        /// </summary>
        public static void SetUniversalProvider(Func<DateTime> timestamp)
        {
            universalProvider = new DelegatingTimestampProvider(timestamp);
        }

        /// <summary>
        /// Gets the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        public static DateTime Universal() => universalProvider.GetTimestamp();

        #endregion Methods

    }
}
