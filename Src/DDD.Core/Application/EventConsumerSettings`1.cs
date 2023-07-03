using EnsureThat;
using System;

namespace DDD.Core.Application
{
    using Domain;

    /// <remarks>
    /// Used for dependency injection
    /// </remarks>
    public class EventConsumerSettings<TContext>
        where TContext : BoundedContext
    {

        #region Constructors

        public EventConsumerSettings(TContext context,
                                     TimeSpan consumationDelay,
                                     long? consumationMax = null)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(consumationDelay, nameof(consumationDelay)).IsGte(TimeSpan.Zero);
            if (consumationMax != null)
                Ensure.That(consumationMax.Value, nameof(consumationMax)).IsGte(0);
            this.Context = context;
            this.ConsumationDelay = consumationDelay;
            this.ConsumationMax = consumationMax;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the associated context.
        /// </summary>
        public TContext Context { get; }

        /// <summary>
        /// Gets the delay between two successive consumations.
        /// </summary>
        public TimeSpan ConsumationDelay { get; }

        /// <summary>
        /// Gets the maximum number of successive consumations.
        /// </summary>
        public long? ConsumationMax { get; }

        #endregion Properties

    }
}
