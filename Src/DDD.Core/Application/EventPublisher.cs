using EnsureThat;
using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Threading;

    public class EventPublisher<TContext> : IEventPublisher<TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public EventPublisher(TContext context, IServiceProvider serviceProvider)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.Context = context;
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        BoundedContext IEventPublisher.Context => this.Context;

        #endregion Properties

        #region Methods

        public async Task PublishAsync<TEvent>(TEvent @event, IMessageContext context) where TEvent : class, IEvent
        {
            Ensure.That(@event, nameof(@event)).IsNotNull();
            await new SynchronizationContextRemover();
            var asyncHandler = this.GetAsyncEventHandler(@event);
            if (asyncHandler != null)
                await asyncHandler.HandleAsync(@event, context);
            else
            {
                var syncHandler = this.GetSyncEventHandler(@event);
                if (syncHandler != null)
                    syncHandler.Handle(@event, context);
            }
        }

        private IAsyncEventHandler GetAsyncEventHandler<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            if (typeof(TEvent) == typeof(IEvent))
            {
                var handlerType = typeof(IAsyncEventHandler<,>).MakeGenericType(@event.GetType(), typeof(TContext));
                return (IAsyncEventHandler)this.serviceProvider.GetService(handlerType);
            }
            return this.serviceProvider.GetService<IAsyncEventHandler<TEvent, TContext>>();
        }

        private ISyncEventHandler GetSyncEventHandler<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            if (typeof(TEvent) == typeof(IEvent))
            {
                var handlerType = typeof(ISyncEventHandler<,>).MakeGenericType(@event.GetType(), typeof(TContext));
                return (ISyncEventHandler)this.serviceProvider.GetService(handlerType);
            }
            return this.serviceProvider.GetService<ISyncEventHandler<TEvent, TContext>>();
        }

        #endregion Methods

    }
}