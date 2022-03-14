using Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            Condition.Requires(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            var handlers = this.GetEventHandlers(@event);
            foreach (var handler in handlers)
                handler.Handle(@event);
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            await new SynchronizationContextRemover();
            var handlers = this.GetAsyncEventHandlers(@event);
            foreach (var handler in handlers)
                await handler.HandleAsync(@event, cancellationToken);
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

        private IEnumerable<IEventHandler> GetEventHandlers<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            if (typeof(TEvent) == typeof(IEvent))
            {
                var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
                return this.serviceProvider.GetServices(handlerType).Cast<IEventHandler>();
            }
            return this.serviceProvider.GetServices<IEventHandler<TEvent>>();
        }

        #endregion Methods

    }
}