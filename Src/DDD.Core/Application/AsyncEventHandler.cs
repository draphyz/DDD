using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling asynchronously events.
    /// </summary>
    public abstract class AsyncEventHandler<TEvent> : IAsyncEventHandler<TEvent>
        where TEvent : class, IEvent
    {

        #region Properties

        Type IAsyncEventHandler.EventType => typeof(TEvent);

        #endregion Properties

        #region Methods

        public abstract Task HandleAsync(TEvent @event, IMessageContext context = null);

        Task IAsyncEventHandler.HandleAsync(IEvent @event, IMessageContext context) => this.HandleAsync((TEvent)@event, context);

        #endregion Methods

    }
}