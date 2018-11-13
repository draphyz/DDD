using Conditions;

namespace DDD.Core.Domain
{
    using Mapping;
    using Serialization;

    public class EventTranslator : IObjectTranslator<IDomainEvent, EventState>
    {
        #region Fields

        private readonly ITextSerializer eventSerializer;

        #endregion Fields

        #region Constructors

        public EventTranslator(ITextSerializer eventSerializer)
        {
            Condition.Requires(eventSerializer, nameof(eventSerializer)).IsNotNull();
            this.eventSerializer = eventSerializer;
        }

        #endregion Constructors

        #region Methods

        public EventState Translate(IDomainEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            return new EventState()
            {
                OccurredOn = @event.OccurredOn,
                EventType = @event.GetType().Name,
                Body = this.eventSerializer.SerializeToString(@event)
            };
        }

        #endregion Methods
    }
}