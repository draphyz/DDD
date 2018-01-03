using System;

namespace DDD.Core.Domain
{
    public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : IDomainEvent
    {
        #region Properties

        Type IDomainEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract void Handle(TEvent @event);

        void IDomainEventHandler.Handle(IDomainEvent @event) => this.Handle((TEvent)@event);

        #endregion Methods
    }
}