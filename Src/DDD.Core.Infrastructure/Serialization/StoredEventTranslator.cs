using Conditions;

namespace DDD.Core.Infrastructure.Serialization
{
    using Mapping;
    using Domain;

    public class StoredEventTranslator : IObjectTranslator<IDomainEvent, StoredEvent>
    {
        #region Fields

        private readonly ISerializer<IDomainEvent> eventSerializer;

        #endregion Fields

        #region Constructors

        public StoredEventTranslator(ISerializer<IDomainEvent> eventSerializer)
        {
            Condition.Requires(eventSerializer, nameof(eventSerializer)).IsNotNull();
            this.eventSerializer = eventSerializer;
        }

        #endregion Constructors

        #region Methods

        public StoredEvent Translate(IDomainEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            return new StoredEvent()
            {
                OccurredOn = @event.OccurredOn,
                EventType = @event.GetType().Name,
                Body = this.eventSerializer.Serialize(@event)
            };
        }

        #endregion Methods
    }
}