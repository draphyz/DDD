using EnsureThat;
using System;
using System.Collections.Generic;
using DDD.Serialization;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;

    public class EventsRegistrationOptions
    {

        #region Fields

        private readonly List<EventConsumerOptions> consumerOptionsCollection = new List<EventConsumerOptions>();

        #endregion Fields

        #region Constructors

        private EventsRegistrationOptions() { }

        #endregion Constructors

        #region Properties

        public IEnumerable<EventConsumerOptions> ConsumerOptionsCollection => this.consumerOptionsCollection;

        /// <summary>
        /// Gets the current serialization format of events.
        /// </summary>
        public SerializationFormat CurrentSerializationFormat { get; private set; }

        #endregion Properties

        #region Classes

        public class Builder : FluentBuilder<EventsRegistrationOptions>
        {

            #region Fields

            private readonly EventsRegistrationOptions options = new EventsRegistrationOptions();

            #endregion Fields

            #region Methods

            public Builder ConfigureConsumerFor<TContext>()
                where TContext : BoundedContext
            {
                return ConfigureConsumerFor<TContext>(_ => { });
            }

            public Builder ConfigureConsumerFor<TContext>(Action<EventConsumerOptions.Builder<TContext>> configureOptions)
                where TContext : BoundedContext
            {
                Ensure.That(configureOptions, nameof(configureOptions)).IsNotNull();
                var builder = new EventConsumerOptions.Builder<TContext>();
                configureOptions(builder);
                options.consumerOptionsCollection.Add(((IObjectBuilder<EventConsumerOptions>)builder).Build());
                return this;
            }

            public Builder ConfigureConsumers(IEnumerable<EventConsumerOptions> optionsCollection)
            {
                Ensure.That(optionsCollection, nameof(optionsCollection)).IsNotNull();
                options.consumerOptionsCollection.AddRange(optionsCollection);
                return this;
            }

            public Builder ConfigureConsumers(params EventConsumerOptions[] optionsCollection)
            {
                Ensure.That(optionsCollection, nameof(optionsCollection)).IsNotNull();
                options.consumerOptionsCollection.AddRange(optionsCollection);
                return this;
            }

            public Builder SetCurrentSerializationFormat(SerializationFormat format)
            {
                this.options.CurrentSerializationFormat = format;
                return this;
            }

            protected override EventsRegistrationOptions Build() => this.options;

            #endregion Methods

        }

        #endregion Classes

    }
}
