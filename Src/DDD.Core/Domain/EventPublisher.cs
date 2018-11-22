using Conditions;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class EventPublisher : IEventPublisher
    {
        #region Fields

        private List<IEventHandler> subscribers = new List<IEventHandler>();

        #endregion Fields

        #region Methods

        public void Publish(IEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            foreach (var subscriber in this.subscribers)
            {
                if (subscriber.EventType.IsAssignableFrom(@event.GetType()))
                    subscriber.Handle(@event);
            }
        }

        public void Subscribe(IEventHandler subscriber)
        {
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            this.subscribers.Add(subscriber);
        }

        public void UnSubscribe(IEventHandler subscriber)
        {
            Condition.Requires(subscriber, nameof(subscriber)).IsNotNull();
            this.subscribers.Remove(subscriber);
        }

        #endregion Methods

    }
}