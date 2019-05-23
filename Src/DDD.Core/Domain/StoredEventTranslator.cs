using Conditions;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    using Mapping;
    using Serialization;

    public class StoredEventTranslator : IObjectTranslator<IEvent, StoredEvent>
    {
        #region Fields

        private readonly ITextSerializer eventSerializer;

        #endregion Fields

        #region Constructors

        public StoredEventTranslator(ITextSerializer eventSerializer)
        {
            Condition.Requires(eventSerializer, nameof(eventSerializer)).IsNotNull();
            this.eventSerializer = eventSerializer;
        }

        #endregion Constructors

        #region Methods

        public StoredEvent Translate(IEvent @event, IDictionary<string, object> options = null)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            return new StoredEvent()
            {
                OccurredOn = @event.OccurredOn,
                EventType = @event.GetType().Name,
                Body = this.eventSerializer.SerializeToString(@event)
            };
        }

        #endregion Methods
    }
}