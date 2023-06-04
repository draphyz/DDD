using EnsureThat;
using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Threading;

    public static class IContextualCommandProcessorExtensions
    {

        #region Methods

        public static void Process<TCommand>(this IContextualCommandProcessor processor,
                                             TCommand command)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Process(command, new MessageContext());
        }

        public static void Process<TCommand>(this IContextualCommandProcessor processor,
                                             TCommand command,
                                             object context)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Process(command, MessageContext.FromObject(context));
        }

        public static Task ProcessAsync<TCommand>(this IContextualCommandProcessor processor,
                                                  TCommand command)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(command, new MessageContext());
        }

        public static Task ProcessAsync<TCommand>(this IContextualCommandProcessor processor,
                                                  TCommand command,
                                                  object context)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(command, MessageContext.FromObject(context));
        }

        public static Task ProcessWithDelayAsync<TCommand>(this IContextualCommandProcessor processor,
                                                           TCommand command,
                                                           TimeSpan delay)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessWithDelayAsync(command, delay, new MessageContext());
        }

        public static async Task ProcessWithDelayAsync<TCommand>(this IContextualCommandProcessor processor,
                                                                 TCommand command,
                                                                 TimeSpan delay,
                                                                 IMessageContext context)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            await new SynchronizationContextRemover();
            var cancellationToken = context.CancellationToken();
            await Task.Delay(delay, cancellationToken);
            await processor.ProcessAsync(command, context);
        }

        public static Task ProcessWithDelayAsync<TCommand>(this IContextualCommandProcessor processor,
                                                           TCommand command,
                                                           TimeSpan delay,
                                                           object context)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessWithDelayAsync(command, delay, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}