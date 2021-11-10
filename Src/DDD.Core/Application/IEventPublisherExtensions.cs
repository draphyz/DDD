using Conditions;
using System.Collections.Generic;
using System.Threading;

namespace DDD.Core.Application
{
    using Domain;
    using Threading;

    public static class IEventPublisherExtensions
    {

        #region Methods

        public static void PublishAll(this IEventPublisher publisher, IEnumerable<IEvent> events)
        {
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            Condition.Requires(events, nameof(events))
                     .IsNotNull()
                     .DoesNotContain(null);
            foreach (var @event in events)
                publisher.Publish(@event);
        }

        public async static void PublishAllAsync(this IEventPublisher publisher, IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            Condition.Requires(events, nameof(events))
                     .IsNotNull()
                     .DoesNotContain(null);
            await new SynchronizationContextRemover();
            foreach (var @event in events)
                await publisher.PublishAsync(@event, cancellationToken);
        }

        #endregion Methods

    }
}