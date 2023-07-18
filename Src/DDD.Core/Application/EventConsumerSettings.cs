using EnsureThat;
using System;

namespace DDD.Core.Application
{
    using Domain;

    public class EventConsumerSettings<TContext>
        where TContext : BoundedContext
    {

        #region Constructors

        public EventConsumerSettings(TimeSpan consumationDelay,
                                     long? consumationMax = null)
        {
            Ensure.That(consumationDelay, nameof(consumationDelay)).IsGte(TimeSpan.Zero);
            if (consumationMax != null)
                Ensure.That(consumationMax.Value, nameof(consumationMax)).IsGte(0);
            this.ContextType = typeof(TContext);
            this.ConsumationDelay = consumationDelay;
            this.ConsumationMax = consumationMax;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type of the associated context.
        /// </summary>
        public Type ContextType { get; }

        /// <summary>
        /// Gets the delay between two successive consumations.
        /// </summary>
        public TimeSpan ConsumationDelay { get; }

        /// <summary>
        /// Gets the maximum number of successive consumations.
        /// </summary>
        public long? ConsumationMax { get; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(ContextType)}={ContextType}, {nameof(ConsumationDelay)}={ConsumationDelay}, {nameof(ConsumationMax)}={ConsumationMax}]";

        #endregion Methods

    }
}
