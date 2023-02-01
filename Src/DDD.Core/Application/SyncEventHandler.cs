using System;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling synchronously events.
    /// </summary>
    public abstract class SyncEventHandler<TEvent> : ISyncEventHandler<TEvent>
        where TEvent : class, IEvent
    {

        #region Properties

        Type ISyncEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract void Handle(TEvent @event, IMessageContext context = null);

        void ISyncEventHandler.Handle(IEvent @event, IMessageContext context) => this.Handle((TEvent)@event, context);

        #endregion Methods

    }
}