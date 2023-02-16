using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling asynchronously events.
    /// </summary>
    public abstract class AsyncEventHandler<TEvent, TContext> : IAsyncEventHandler<TEvent, TContext>
        where TEvent : class, IEvent
        where TContext : BoundedContext, new()
    {

        #region Properties

        public TContext Context { get; } = new TContext();

        BoundedContext IAsyncEventHandler.Context => this.Context;

        Type IAsyncEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract Task HandleAsync(TEvent @event, IMessageContext context = null);

        Task IAsyncEventHandler.HandleAsync(IEvent @event, IMessageContext context) => this.HandleAsync((TEvent)@event, context);

        #endregion Methods

    }
}