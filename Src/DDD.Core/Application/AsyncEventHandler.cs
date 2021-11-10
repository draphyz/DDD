using System;
using System.Threading;
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

        public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);

        Task IAsyncEventHandler.HandleAsync(IEvent @event, CancellationToken cancellationToken) => this.HandleAsync((TEvent)@event, cancellationToken);

        #endregion Methods
    }
}