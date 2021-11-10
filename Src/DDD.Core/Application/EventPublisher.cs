using Conditions;
using System;
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
            if (typeof(TEvent) == typeof(IEvent))
            {
                var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
                var handlers = this.serviceProvider.GetServices(handlerType).Cast<IEventHandler>();
                foreach (var handler in handlers)
                    handler.Handle(@event);
            }
            else
            {
                var handlers = this.serviceProvider.GetServices<IEventHandler<TEvent>>();
                foreach (var handler in handlers)
                    handler.Handle(@event);
            }
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            await new SynchronizationContextRemover();
            if (typeof(TEvent) == typeof(IEvent))
            {
                var handlerType = typeof(IAsyncEventHandler<>).MakeGenericType(@event.GetType());
                var handlers = this.serviceProvider.GetServices(handlerType).Cast<IAsyncEventHandler>();
                foreach (var handler in handlers)
                    await handler.HandleAsync(@event, cancellationToken);
            }
            else
            {
                var handlers = this.serviceProvider.GetServices<IAsyncEventHandler<TEvent>>();
                foreach (var handler in handlers)
                    await handler.HandleAsync(@event, cancellationToken);
            }
        }

        #endregion Methods
    }
}