using EnsureThat;
using DDD.Core.Domain;
using System.Runtime.Serialization;

namespace DDD.Core.Application
{
    [DataContract()]
    public class EventConsumerSettings<TContext>
        where TContext : BoundedContext, new()
    {
        #region Fields

        private readonly TContext context;

        #endregion Fields

        #region Constructors

        public EventConsumerSettings(short consumationDelay,
                                     long? consumationMax = null)
        {
            Ensure.That(consumationDelay, nameof(consumationDelay)).IsGte((short)0);
            if (consumationMax != null)
                Ensure.That(consumationMax.Value, nameof(consumationMax)).IsGte(0);
            this.ConsumationDelay = consumationDelay;
            this.ConsumationMax = consumationMax;
            this.context = new TContext();
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

        /// <summary>
        /// Gets the associated context.
        /// </summary>
        public TContext Context => this.context;

        #endregion Properties
    }
}
