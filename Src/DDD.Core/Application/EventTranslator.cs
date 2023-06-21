using EnsureThat;
using System;
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

        public override Event Translate(IEvent @event, IMappingContext context)
        {
            Ensure.That(@event, nameof(@event)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue("EventId", out Guid eventId);
            context.TryGetValue("StreamId", out string streamId);
            context.TryGetValue("StreamType", out string streamType);
            context.TryGetValue("IssuedBy", out string issuedBy);
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