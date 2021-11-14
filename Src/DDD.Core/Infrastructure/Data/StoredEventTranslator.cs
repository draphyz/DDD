using Conditions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    using Mapping;
    using DDD.Serialization;

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
            var eventType = @event.GetType();
            return new StoredEvent()
            {
                OccurredOn = @event.OccurredOn,
                EventType = $"{eventType.FullName}, {eventType.Assembly.GetName().Name}",
                Version = ToVersion(eventType.FullName),
                Body = this.eventSerializer.SerializeToString(@event)
            };
        }

        private static byte ToVersion(string fullName)
        {
            byte version = 1;
            var match = Regex.Match(fullName, @".(Version|V)\d+.");
            if (match.Success)
            {
                var value = Regex.Replace(match.Value, "(Version|V)", string.Empty)
                                 .Replace(".", string.Empty);
                version = byte.Parse(value);
            }
            return version;
        }

        #endregion Methods

    }
}