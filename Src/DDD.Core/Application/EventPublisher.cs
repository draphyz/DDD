using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Threading;

    public class EventPublisher : IEventPublisher
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public EventPublisher(IServiceProvider serviceProvider)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public async Task PublishAsync<TEvent>(TEvent @event, IMessageContext context = null) where TEvent : class, IEvent
        {
            Ensure.That(@event, nameof(@event)).IsNotNull();
            await new SynchronizationContextRemover();
            var syncHandlers = this.GetSyncEventHandlers(@event);
            foreach (var handler in syncHandlers)
                handler.Handle(@event, context);
            var asyncHandlers = this.GetAsyncEventHandlers(@event);
            foreach (var handler in asyncHandlers)
                await handler.HandleAsync(@event, context);
        }

        private IEnumerable<IAsyncEventHandler> GetAsyncEventHandlers<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            if (typeof(TEvent) == typeof(IEvent))
            {
                var handlerType = typeof(IAsyncEventHandler<>).MakeGenericType(@event.GetType());
                return this.serviceProvider.GetServices(handlerType).Cast<IAsyncEventHandler>();
            }
            return this.serviceProvider.GetServices<IAsyncEventHandler<TEvent>>();
        }

        private IEnumerable<ISyncEventHandler> GetSyncEventHandlers<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            if (typeof(TEvent) == typeof(IEvent))
            {
                var handlerType = typeof(ISyncEventHandler<>).MakeGenericType(@event.GetType());
                return this.serviceProvider.GetServices(handlerType).Cast<ISyncEventHandler>();
            }
            return this.serviceProvider.GetServices<ISyncEventHandler<TEvent>>();
        }

        #endregion Methods

    }
}