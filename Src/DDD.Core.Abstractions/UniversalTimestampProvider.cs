using System;

namespace DDD
{
    /// <summary>
    /// Provides timestamps based on the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
    /// </summary>
    public class UniversalTimestampProvider : ITimestampProvider
    {

        #region Methods

        public DateTime GetTimestamp() => DateTime.UtcNow;

        #endregion Methods

    }
}