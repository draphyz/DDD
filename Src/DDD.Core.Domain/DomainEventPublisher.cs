using Conditions;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        #region Fields

        private List<IDomainEventHandler> subscribers = new List<IDomainEventHandler>();

        #endregion Fields

        #region Methods

        public void Publish(IDomainEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            foreach (var subscriber in this.subscribers)
            {
                if (subscriber.EventType.IsAssignableFrom(@event.GetType()))
                    subscriber.Handle(@event);
            }
        }

        public void Subscribe(IDomainEventHandler subscriber)
        {
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            this.subscribers.Add(subscriber);
        }

        public void UnSubscribe(IDomainEventHandler subscriber)
        {
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            this.subscribers.Remove(subscriber);
        }

        #endregion Methods

    }
}