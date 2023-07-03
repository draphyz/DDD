using EnsureThat;
using System.Runtime.Serialization;

namespace DDD.Core.Application
{
    /// <remarks>
    /// Used for serialization
    /// </remarks>
    [DataContract]
    public class EventConsumerSettings
    {

        #region Constructors

        public EventConsumerSettings(string context,
                                     short consumationDelay,
                                     long? consumationMax = null)
        {
            Ensure.That(context, nameof(context)).IsNotNullOrWhiteSpace();
            Ensure.That(consumationDelay, nameof(consumationDelay)).IsGte((short)0);
            if (consumationMax != null)
                Ensure.That(consumationMax.Value, nameof(consumationMax)).IsGte(0);
            this.Context = context;
            this.ConsumationDelay = consumationDelay;
            this.ConsumationMax = consumationMax;
        }

        /// <remarks>
        /// For serialization
        /// </remarks>
        private EventConsumerSettings() { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the associated context.
        /// </summary>
        [DataMember(Order = 1)]
        public string Context { get; private set; }

        /// <summary>
        /// Gets the delay in seconds between two successive consumations.
        /// </summary>
        [DataMember(Order = 2)]
        public short ConsumationDelay { get; private set; }

        /// <summary>
        /// Gets the maximum number of successive consumations.
        /// </summary>
        [DataMember(Order = 3)]
        public long? ConsumationMax { get; private set; }

        #endregion Properties

    }
}
