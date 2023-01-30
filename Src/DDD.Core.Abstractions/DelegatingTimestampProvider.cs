using EnsureThat;
using System;

namespace DDD
{
    /// <summary>
    /// Adapter that converts a delegate into an object that implements ITimestampProvider.
    /// </summary>
    public class DelegatingTimestampProvider : ITimestampProvider
    {

        #region Fields

        private readonly Func<DateTime> timestamp;

        #endregion Fields

        #region Constructors

        public DelegatingTimestampProvider(Func<DateTime> timestamp)
        {
            Ensure.That(timestamp, nameof(timestamp)).IsNotNull();
            this.timestamp = timestamp;
        }

        #endregion Constructors

        #region Methods

        public DateTime GetTimestamp() => this.timestamp();

        #endregion Methods

    }
}