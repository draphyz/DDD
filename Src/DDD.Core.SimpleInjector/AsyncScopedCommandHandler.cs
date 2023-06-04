using SimpleInjector;
using SimpleInjector.Lifestyles;
using EnsureThat;
using System;
using System.Transactions;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using Threading;

    /// <summary>
    /// A decorator that defines a scope around the asynchronous execution of a command.
    /// In the context of event handling, this decorator updates the position of event stream in the same transaction as the command. 
    /// </summary>
    public class AsyncScopedCommandHandler<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly Container container;
        private readonly Func<IAsyncCommandHandler<TCommand>> handlerProvider;

        #endregion Fields

        #region Constructors

        public AsyncScopedCommandHandler(Func<IAsyncCommandHandler<TCommand>> handlerProvider,
                                         Container container)
        {
            Ensure.That(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Ensure.That(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(TCommand command, IMessageContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            await new SynchronizationContextRemover();
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var handler = this.handlerProvider();
                    await handler.HandleAsync(command, context);
                    if (context.IsEventHandling()) // Exception to the rule "One transaction per command" to avoid to handle the same event more than once
                        await UpdateEventStreamPositionAsync(context);
                    scope.Complete();
                }
            }
        }

        private async Task UpdateEventStreamPositionAsync(IMessageContext context)
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
                var handlerType = typeof(IAsyncCommandHandler<,>).MakeGenericType(typeof(UpdateEventStreamPosition), boundedContext.GetType());
                dynamic handler = this.container.GetInstance(handlerType);
                await handler.HandleAsync(command, context);
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
                var handlerType = typeof(IAsyncCommandHandler<,>).MakeGenericType(typeof(UpdateFailedEventStreamPosition), boundedContext.GetType());
                dynamic handler = this.container.GetInstance(handlerType);
                await handler.HandleAsync(command, context);
                failedStream.StreamPosition = @event.EventId;
            }
        }

        #endregion Methods

    }
}
