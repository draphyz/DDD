using System;
using System.Threading.Tasks;
using EnsureThat;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling asynchronously events.
    /// </summary>
    public abstract class AsyncEventHandler<TEvent, TContext> : IAsyncEventHandler<TEvent, TContext>
        where TEvent : class, IEvent
        where TContext : BoundedContext
    {

        #region Constructors

        protected AsyncEventHandler(TContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            this.Context = context;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; } 

        BoundedContext IAsyncEventHandler.Context => this.Context;

        Type IAsyncEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract Task HandleAsync(TEvent @event, IMessageContext context);

        Task IAsyncEventHandler.HandleAsync(IEvent @event, IMessageContext context) => this.HandleAsync((TEvent)@event, context);

        #endregion Methods

    }
}