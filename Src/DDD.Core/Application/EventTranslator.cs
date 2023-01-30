using EnsureThat;
using System;
using System.Collections.Generic;
using DDD.Serialization;

namespace DDD.Core.Application
{
    using Domain;
    using Mapping;
    using Collections;

    public class EventTranslator : ObjectTranslator<IEvent, Event>
    {

        #region Fields

        private readonly ITextSerializer eventSerializer;

        #endregion Fields

        #region Constructors

        public EventTranslator(ITextSerializer eventSerializer)
        {
            Ensure.That(eventSerializer, nameof(eventSerializer)).IsNotNull();
            this.eventSerializer = eventSerializer;
        }

        #endregion Constructors

        #region Methods

        public override Event Translate(IEvent @event, IDictionary<string, object> context = null)
        {
            Ensure.That(@event, nameof(@event)).IsNotNull();
            Guid eventId = default;
            string streamId = null, streamType = null, issuedBy = null;
            if (context != null)
            {
                context.TryGetValue("EventId", out eventId);
                context.TryGetValue("StreamId", out streamId);
                context.TryGetValue("StreamType", out streamType);
                context.TryGetValue("IssuedBy", out issuedBy);
            }
            var eventType = @event.GetType();
            return new Event()
            {
                EventId = eventId,
                EventType = eventType.ShortAssemblyQualifiedName(),
                OccurredOn = @event.OccurredOn,
                Body = this.eventSerializer.SerializeToString(@event),
                BodyFormat = this.eventSerializer.Format.ToString().ToUpper(),
                StreamId = streamId,
                StreamType = streamType,
                IssuedBy = issuedBy
            };
        }

        #endregion Methods

    }
}