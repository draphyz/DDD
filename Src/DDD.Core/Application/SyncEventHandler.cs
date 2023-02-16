using System;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling synchronously events.
    /// </summary>
    public abstract class SyncEventHandler<TEvent, TContext> : ISyncEventHandler<TEvent, TContext>
        where TEvent : class, IEvent
        where TContext : BoundedContext, new()
    {

        #region Properties

        public TContext Context { get; } = new TContext();

        BoundedContext ISyncEventHandler.Context => this.Context;

        Type ISyncEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract void Handle(TEvent @event, IMessageContext context = null);

        void ISyncEventHandler.Handle(IEvent @event, IMessageContext context) => this.Handle((TEvent)@event, context);

        #endregion Methods

    }
}