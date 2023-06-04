using System;
using EnsureThat;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling synchronously events.
    /// </summary>
    public abstract class SyncEventHandler<TEvent, TContext> : ISyncEventHandler<TEvent, TContext>
        where TEvent : class, IEvent
        where TContext : BoundedContext
    {

        #region Constructors

        protected SyncEventHandler(TContext context) 
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            this.Context = context;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        BoundedContext ISyncEventHandler.Context => this.Context;

        Type ISyncEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract void Handle(TEvent @event, IMessageContext context);

        void ISyncEventHandler.Handle(IEvent @event, IMessageContext context) => this.Handle((TEvent)@event, context);

        #endregion Methods

    }
}