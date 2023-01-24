using Conditions;
using System;
using System.Threading;

namespace DDD.Core.Application
{
    using Domain;
    using Collections;

    public static class IMessageContextExtensions
    {

        #region Methods

        public static void AddCancellationToken(this IMessageContext context, CancellationToken cancellationToken)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            context.Add(MessageContextInfo.CancellationToken, cancellationToken);
        }

        public static void AddEvent(this IMessageContext context, Event @event)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            context.Add(MessageContextInfo.Event, @event);
        }

        public static void AddFailedStream(this IMessageContext context, FailedEventStream stream)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            context.Add(MessageContextInfo.FailedStream, stream);
        }

        public static void AddStream(this IMessageContext context, EventStream stream)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            context.Add(MessageContextInfo.Stream, stream);
        }

        public static CancellationToken CancellationToken(this IMessageContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.CancellationToken, out CancellationToken cancellationToken);
            return cancellationToken;
        }

        public static Event Event(this IMessageContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.Event, out Event @event);
            return @event;
        }

        public static FailedEventStream FailedStream(this IMessageContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.FailedStream, out FailedEventStream stream);
            return stream;
        }

        public static EventStream Stream(this IMessageContext context)
        {
            Condition.Requires(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.Stream, out EventStream stream);
            return stream;
        }

        #endregion Methods

    }
}
