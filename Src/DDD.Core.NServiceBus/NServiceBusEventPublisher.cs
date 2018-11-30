using NServiceBus;
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

        public async Task PublishAsync(IEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            await new SynchronizationContextRemover();
            await this.session.Publish(@event);
        }

        #endregion Methods

    }
}
