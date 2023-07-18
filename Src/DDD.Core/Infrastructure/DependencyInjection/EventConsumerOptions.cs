using EnsureThat;
using System;
using System.Runtime.Serialization;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;

    [DataContract]
    public class EventConsumerOptions 
    {

        #region Constructors

        private EventConsumerOptions(string contextType) 
        { 
            this.ContextType = contextType;
        }

        /// <remarks>
        /// For serialization
        /// </remarks>
        private EventConsumerOptions() { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type name of the associated context.
        /// </summary>
        [DataMember(Order = 1)]
        public string ContextType { get; private set; }

        /// <summary>
        /// Gets the delay in seconds between two successive consumations.
        /// </summary>
        [DataMember(Order = 2)]
        public double ConsumationDelay { get; private set; }

        /// <summary>
        /// Gets the maximum number of successive consumations.
        /// </summary>
        [DataMember(Order = 3)]
        public long? ConsumationMax { get; private set; }

        #endregion Properties

        #region Classes

        public class Builder<TContext> : IObjectBuilder<EventConsumerOptions>
            where TContext : BoundedContext
        {

            #region Fields

            private readonly EventConsumerOptions options;

            #endregion Fields

            public Builder() 
            { 
                this.options = new EventConsumerOptions(typeof(TContext).ShortAssemblyQualifiedName());
            }

            #region Methods

            public Builder<TContext> SetConsumationDelayInSeconds(double consumationDelay)
            {
                Ensure.That(consumationDelay, nameof(consumationDelay)).IsGte(0);
                options.ConsumationDelay = consumationDelay;
                return this;
            }

            public Builder<TContext> SetConsumationDelay(TimeSpan consumationDelay)
            {
                Ensure.That(consumationDelay, nameof(consumationDelay)).IsGte(TimeSpan.Zero);
                options.ConsumationDelay = consumationDelay.TotalSeconds;
                return this;
            }

            public Builder<TContext> SetConsumationMax(long consumationMax)
            {
                Ensure.That(consumationMax, nameof(consumationMax)).IsGte(0);
                options.ConsumationMax = consumationMax;
                return this;
            }

            EventConsumerOptions IObjectBuilder<EventConsumerOptions>.Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}