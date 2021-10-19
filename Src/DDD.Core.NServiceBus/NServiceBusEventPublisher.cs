using NServiceBus;
using System.Threading;
using System.Threading.Tasks;
using Conditions;

namespace DDD.Core.Infrastructure.Messaging
{
    using Domain;
    using Threading;

    public class NServiceBusEventPublisher : IAsyncEventPublisher
    {

        #region Fields

        private readonly IMessageSession session;

        #endregion Fields

        #region Constructors

        public NServiceBusEventPublisher(IMessageSession session)
        {
            Condition.Requires(session, nameof(session)).IsNotNull();
            this.session = session;
        }

        #endregion Constructors

        #region Methods

        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            await new SynchronizationContextRemover();
            // Cancellation token support will be implemented in NServiceBus 8 (not yet released)
            await this.session.Publish(@event);
        }

        #endregion Methods

    }
}
