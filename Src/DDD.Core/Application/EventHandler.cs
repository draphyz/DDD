using System;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling events.
    /// </summary>
    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent 
    {
        #region Properties

        Type IEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract void Handle(TEvent @event);

        void IEventHandler.Handle(IEvent @event) => this.Handle((TEvent)@event);

        #endregion Methods
    }
}