using EnsureThat;
using System;
using System.Threading.Tasks;
using DDD.Validation;

namespace DDD.Core.Application
{
    using Threading;

    public static class ICommandProcessorExtensions
    {

        #region Methods

        public static void Process<TCommand>(this ICommandProcessor processor,
                                             TCommand command)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Process(command, new MessageContext());
        }

        public static void Process<TCommand>(this ICommandProcessor processor,
                                             TCommand command,
                                             object context)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Process(command, MessageContext.FromObject(context));
        }

        public static void Process(this ICommandProcessor processor,
                                   ICommand command)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Process(command, new MessageContext());
        }

        public static void Process(this ICommandProcessor processor,
                                   ICommand command,
                                   object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            processor.Process(command, MessageContext.FromObject(context));
        }

        public static Task ProcessAsync<TCommand>(this ICommandProcessor processor,
                                                  TCommand command)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(command, new MessageContext());
        }

        public static Task ProcessAsync<TCommand>(this ICommandProcessor processor,
                                                  TCommand command,
                                                  object context)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(command, MessageContext.FromObject(context));
        }

        public static Task ProcessAsync(this ICommandProcessor processor,
                                        ICommand command)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(command, new MessageContext());
        }

        public static Task ProcessAsync(this ICommandProcessor processor,
                                        ICommand command,
                                        object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(command, MessageContext.FromObject(context));
        }

        public static Task ProcessWithDelayAsync<TCommand>(this ICommandProcessor processor,
                                                           TCommand command,
                                                           TimeSpan delay)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessWithDelayAsync(command, delay, new MessageContext());
        }

        public static async Task ProcessWithDelayAsync<TCommand>(this ICommandProcessor processor,
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

        public static Task ProcessWithDelayAsync<TCommand>(this ICommandProcessor processor,
                                                           TCommand command,
                                                           TimeSpan delay,
                                                           object context)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessWithDelayAsync(command, delay, MessageContext.FromObject(context));
        }

        public static Task ProcessWithDelayAsync(this ICommandProcessor processor,
                                                 ICommand command,
                                                 TimeSpan delay)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessWithDelayAsync(command, delay, new MessageContext());
        }

        public static async Task ProcessWithDelayAsync(this ICommandProcessor processor,
                                                       ICommand command,
                                                       TimeSpan delay,
                                                       IMessageContext context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            await new SynchronizationContextRemover();
            var cancellationToken = context.CancellationToken();
            await Task.Delay(delay, cancellationToken);
            await processor.ProcessAsync(command, context);
        }

        public static Task ProcessWithDelayAsync<TCommand>(this ICommandProcessor processor,
                                                           ICommand command,
                                                           TimeSpan delay,
                                                           object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessWithDelayAsync(command, delay, MessageContext.FromObject(context));
        }

        public static ValidationResult Validate<TCommand>(this ICommandProcessor processor,
                                                          TCommand command)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(command, new ValidationContext());
        }

        public static ValidationResult Validate<TCommand>(this ICommandProcessor processor, 
                                                          TCommand command, 
                                                          object context) 
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(command, ValidationContext.FromObject(context));
        }

        public static ValidationResult Validate(this ICommandProcessor processor,
                                                ICommand command)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(command, new ValidationContext());
        }

        public static ValidationResult Validate(this ICommandProcessor processor,
                                                ICommand command,
                                                object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(command, ValidationContext.FromObject(context));
        }

        public static Task<ValidationResult> ValidateAsync<TCommand>(this ICommandProcessor processor,
                                                                     TCommand command)
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(command, new ValidationContext());
        }

        public static Task<ValidationResult> ValidateAsync<TCommand>(this ICommandProcessor processor, 
                                                                     TCommand command, 
                                                                     object context) 
            where TCommand : class, ICommand
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(command, ValidationContext.FromObject(context));
        }

        public static Task<ValidationResult> ValidateAsync(this ICommandProcessor processor,
                                                           ICommand command)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(command, new ValidationContext());
        }

        public static Task<ValidationResult> ValidateAsync(this ICommandProcessor processor,
                                                           ICommand command,
                                                           object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(command, ValidationContext.FromObject(context));
        }

        #endregion Methods

    }
}
