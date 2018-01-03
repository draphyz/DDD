using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        #region Fields

        private bool publishing = false;
        private List<IDomainEventHandler> subscribers = new List<IDomainEventHandler>();

        #endregion Fields

        #region Constructors

        public DomainEventPublisher()
        {
        }

        public DomainEventPublisher(IEnumerable<IDomainEventHandler> subscribers)
        {
            Condition.Requires(subscribers, nameof(subscribers))
                     .IsNotNull()
                     .DoesNotContain(null);
            this.subscribers.AddRange(subscribers);
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<IDomainEventHandler> AllSubscribers() => this.subscribers.AsReadOnly();

        public bool HasSubscribers() => this.subscribers.Count > 0;

        public void Publish(IDomainEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            if (!this.publishing && this.HasSubscribers())
            {
                try
                {
                    this.publishing = true;
                    foreach (var subscriber in this.subscribers)
                    {
                        if (subscriber.EventType.IsAssignableFrom(@event.GetType()))
                        {
                            subscriber.Handle(@event);
                        }
                    }
                }
                finally
                {
                    this.publishing = false;
                }
            }
        }

        public void PublishAll(IEnumerable<IDomainEvent> events)
        {
            Condition.Requires(events, nameof(events))
                     .IsNotNull()
                     .DoesNotContain(null);
            foreach (var @event in events)
            {
                this.Publish(@event);
            }
        }

        public void Register(IDomainEventHandler subscriber)
        {
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            if (!this.publishing)
            {
                this.subscribers.Add(subscriber);
            }
        }

        public void Register<TEvent>(Action<TEvent> subscriber) where TEvent : IDomainEvent
        {
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            Register(new DelegatingEventHandler<TEvent>(subscriber));
        }

        public void RegisterAll(IEnumerable<IDomainEventHandler> subscribers)
        {
            Condition.Requires(subscribers, nameof(subscribers))
                     .IsNotNull()
                     .DoesNotContain(null);
            if (!this.publishing)
            {
                this.subscribers.AddRange(subscribers);
            }
        }

        public void Unregister(IDomainEventHandler subscriber)
        {
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            if (!this.publishing)
            {
                this.subscribers.Remove(subscriber);
            }
        }

        public void UnregisterAll()
        {
            if (!this.publishing)
            {
                this.subscribers.Clear();
            }
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