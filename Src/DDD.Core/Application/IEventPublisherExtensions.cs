using System.Threading.Tasks;
using EnsureThat;

namespace DDD.Core.Application
{
    using Domain;

    public static class IEventPublisherExtensions
    {

        #region Methods

        public static Task PublishAsync<TEvent>(this IEventPublisher publisher, 
                                                TEvent @event) 
            where TEvent : class, IEvent
        {
            Ensure.That(publisher, nameof(publisher)).IsNotNull();
            return publisher.PublishAsync(@event, new MessageContext());
        }

        public static Task PublishAsync<TEvent>(this IEventPublisher publisher,
                                                TEvent @event,
                                                object context)
            where TEvent : class, IEvent
        {
            Ensure.That(publisher, nameof(publisher)).IsNotNull();
            return publisher.PublishAsync(@event, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}
