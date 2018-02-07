using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public static class IDomainEventPublisherExtensions
    {

        #region Methods

        public static void PublishAll(this IDomainEventPublisher publisher, IEnumerable<IDomainEvent> events)
        {
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            Condition.Requires(events, nameof(events))
                     .IsNotNull()
                     .DoesNotContain(null);
            foreach (var @event in events)
                publisher.Publish(@event);
        }

        public static void Subscribe<TEvent>(this IDomainEventPublisher publisher, Action<TEvent> subscriber) where TEvent : IDomainEvent
        {
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            publisher.Subscribe(new DelegatingEventHandler<TEvent>(subscriber));
        }

        public static void SubscribeAll(this IDomainEventPublisher publisher, IEnumerable<IDomainEventHandler> subscribers)
        {
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            Condition.Requires(subscribers, nameof(subscribers))
                     .IsNotNull()
                     .DoesNotContain(null);
            foreach (var subscriber in subscribers)
                publisher.Subscribe(subscriber);
        }

        public static void UnSubscribeAll(this IDomainEventPublisher publisher, IEnumerable<IDomainEventHandler> subscribers)
        {
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            Condition.Requires(subscribers, nameof(subscribers))
                     .IsNotNull()
                     .DoesNotContain(null);
            foreach (var subscriber in subscribers)
                publisher.UnSubscribe(subscriber);
        }

        #endregion Methods

        #region Classes

        private class DelegatingEventHandler<TEvent> : DomainEventHandler<TEvent>
            where TEvent : IDomainEvent
        {

            #region Fields

            private readonly Action<TEvent> handle;

            #endregion Fields

            #region Constructors

            public DelegatingEventHandler(Action<TEvent> handle)
            {
                Condition.Requires(handle, nameof(handle)).IsNotNull();
                this.handle = handle;
            }

            #endregion Constructors

            #region Methods

            public override void Handle(TEvent @event)
            {
                this.handle(@event);
            }

            #endregion Methods

        }

        #endregion Classes

    }
}
