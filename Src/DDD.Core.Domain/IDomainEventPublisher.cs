using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public interface IDomainEventPublisher
    {
        void Publish(IDomainEvent @event);

        void PublishAll(IEnumerable<IDomainEvent> events);

        void Register(IDomainEventHandler subscriber);

        void Register<TEvent>(Action<TEvent> subscriber) where TEvent : IDomainEvent;

        void RegisterAll(IEnumerable<IDomainEventHandler> subscribers);

        void Unregister(IDomainEventHandler subscriber);

        void UnregisterAll();
    }
}