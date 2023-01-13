using Conditions;
using System.Runtime.Serialization;

namespace DDD.Core.Application
{
    [DataContract()]
    public class EventConsumerSettings
    {

        #region Constructors

        public EventConsumerSettings(short consumationDelay,
                                     long? consumationMax = null)
        {
            Condition.Requires(consumationDelay, nameof(consumationDelay)).IsGreaterOrEqual(0);
            if (consumationMax != null)
                Condition.Requires(consumationMax, nameof(consumationMax)).IsGreaterThan(0);
            this.ConsumationDelay = consumationDelay;
            this.ConsumationMax = consumationMax;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the delay in seconds between two successive consumations.
        /// </summary>
        [DataMember(Order = 1)]
        public short ConsumationDelay { get; }

        /// <summary>
        /// Gets the maximum number of successive consumations.
        /// </summary>
        [DataMember(Order = 2)]
        public long? ConsumationMax { get; }

        #endregion Properties

    }
}
