using EnsureThat;
using System.Threading;

namespace DDD.Core.Application
{
    using Collections;
    using Domain;

    public static class IMessageContextExtensions
    {

        #region Methods

        public static void AddBoundedContext(this IMessageContext context, BoundedContext boundedContext)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(boundedContext, nameof(boundedContext)).IsNotNull();
            context.Add(MessageContextInfo.BoundedContext, boundedContext);
        }

        public static void AddCancellationToken(this IMessageContext context, CancellationToken cancellationToken)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.Add(MessageContextInfo.CancellationToken, cancellationToken);
        }

        public static void AddEvent(this IMessageContext context, Event @event)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(@event, nameof(@event)).IsNotNull();
            context.Add(MessageContextInfo.Event, @event);
        }

        public static void AddFailedStream(this IMessageContext context, FailedEventStream stream)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(stream, nameof(stream)).IsNotNull();
            context.Add(MessageContextInfo.FailedStream, stream);
        }

        public static void AddStream(this IMessageContext context, EventStream stream)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(stream, nameof(stream)).IsNotNull();
            context.Add(MessageContextInfo.Stream, stream);
        }

        public static BoundedContext BoundedContext(this IMessageContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.BoundedContext, out BoundedContext boundedContext);
            return boundedContext;
        }

        public static CancellationToken CancellationToken(this IMessageContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.CancellationToken, out CancellationToken cancellationToken);
            return cancellationToken;
        }

        public static Event Event(this IMessageContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.Event, out Event @event);
            return @event;
        }

        public static FailedEventStream FailedStream(this IMessageContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.FailedStream, out FailedEventStream stream);
            return stream;
        }

        public static bool IsEventHandling(this IMessageContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            return context.ContainsKey(MessageContextInfo.Event);
        }

        public static EventStream Stream(this IMessageContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(MessageContextInfo.Stream, out EventStream stream);
            return stream;
        }

        #endregion Methods
    }
}
