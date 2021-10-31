using Conditions;
using System;

namespace DDD
{
    public class DelegatingTimestampProvider : ITimestampProvider
    {

        #region Fields

        private readonly Func<DateTime> timestamp;

        #endregion Fields

        #region Constructors

        public DelegatingTimestampProvider(Func<DateTime> timestamp)
        {
            Condition.Requires(timestamp, nameof(timestamp)).IsNotNull();
            this.timestamp = timestamp;
        }

        #endregion Constructors

        #region Methods

        public static ITimestampProvider CreateLocal() => new DelegatingTimestampProvider(() => DateTime.Now);

        public static ITimestampProvider CreateUniversal() => new DelegatingTimestampProvider(() => DateTime.UtcNow);

        public DateTime GetTimestamp() => this.timestamp();

        #endregion Methods

    }
}