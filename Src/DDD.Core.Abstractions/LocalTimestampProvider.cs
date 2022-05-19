using System;

namespace DDD
{
    /// <summary>
    /// Provides timestamps based on the current date and time on this computer, expressed as the local time.
    /// </summary>
    public class LocalTimestampProvider : ITimestampProvider
    {
        #region Methods

        public DateTime GetTimestamp() => DateTime.Now;

        #endregion Methods
    }
}
