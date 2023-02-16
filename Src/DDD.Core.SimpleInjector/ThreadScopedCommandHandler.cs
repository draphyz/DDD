using SimpleInjector;
using SimpleInjector.Lifestyles;
using EnsureThat;
using System;
using System.Transactions;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;

    /// <summary>
    /// A decorator that defines a scope around the synchronous execution of a command.
    /// In the context of event handling, this decorator updates the position of event stream in the same transaction as the command. 
    /// </summary>
    public class ThreadScopedCommandHandler<TCommand> : ISyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly Container container;
        private readonly Func<ISyncCommandHandler<TCommand>> handlerProvider;

        #endregion Fields

        #region Constructors

        public ThreadScopedCommandHandler(Func<ISyncCommandHandler<TCommand>> handlerProvider,
                                          Container container)
        {
            Ensure.That(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Ensure.That(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
        }

        #endregion Constructors 

        #region Methods<

        public void Handle(TCommand command, IMessageContext context = null)
        {
            using (ThreadScopedLifestyle.BeginScope(container))
            {
                using (var scope = new TransactionScope())
                {
                    var handler = this.handlerProvider();
                    handler.Handle(command, context);
                    if (context?.IsEventHandling() == true) // Exception to the rule "One transaction per command" to avoid to handle the same event more than once
                        UpdateEventStreamPosition(context);
                    scope.Complete();
                }
            }
        }

        private void UpdateEventStreamPosition(IMessageContext context)
        {
            var @event = context.Event();
            var boundedContext = context.BoundedContext();
            var stream = context.Stream();
            if (stream != null)
            {
                var command = new UpdateEventStreamPosition
                {
                    Position = @event.EventId,
                    Type = stream.Type,
                    Source = stream.Source
                };
                var handlerType = typeof(ISyncCommandHandler<,>).MakeGenericType(typeof(UpdateEventStreamPosition), boundedContext.GetType());
                dynamic handler = this.container.GetInstance(handlerType);
                handler.Handle(command, context);
                stream.Position = @event.EventId;
            }
            else
            {
                var failedStream = context.FailedStream();
                var command = new UpdateFailedEventStreamPosition
                {
                    Position = @event.EventId,
                    Id = failedStream.StreamId,
                    Type = failedStream.StreamType,
                    Source = failedStream.StreamSource
                };
                var handlerType = typeof(ISyncCommandHandler<,>).MakeGenericType(typeof(UpdateFailedEventStreamPosition), boundedContext.GetType());
                dynamic handler = this.container.GetInstance(handlerType);
                handler.Handle(command, context);
                failedStream.StreamPosition = @event.EventId;
            }
        }

        #endregion Methods

    }
}
