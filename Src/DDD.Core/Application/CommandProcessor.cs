using EnsureThat;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Validation;

    /// <summary>
    /// The default command processor for processing and validating commands of any type.  
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public CommandProcessor(IServiceProvider serviceProvider)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public IContextualCommandProcessor<TContext> In<TContext>(TContext context) where TContext : BoundedContext
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            return new ContextualCommandProcessor<TContext>(this.serviceProvider, context);
        }

        public IContextualCommandProcessor In(BoundedContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            var processorType = typeof(ContextualCommandProcessor<>).MakeGenericType(context.GetType());
            return (IContextualCommandProcessor)Activator.CreateInstance(processorType, this.serviceProvider, context);
        }

        public void Process<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<ISyncCommandHandler<TCommand>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(ISyncCommandHandler<TCommand>)} could not be found.");
            handler.Handle(command, context);
        }

        public Task ProcessAsync<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<IAsyncCommandHandler<TCommand>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(IAsyncCommandHandler<TCommand>)} could not be found.");
            return handler.HandleAsync(command, context);
        }

        public ValidationResult Validate<TCommand>(TCommand command, string ruleSet = null) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var validator = this.serviceProvider.GetService<ISyncObjectValidator<TCommand>>();
            if (validator == null) throw new InvalidOperationException($"The command validator for type {typeof(ISyncObjectValidator<TCommand>)} could not be found.");
            return validator.Validate(command, ruleSet);
        }

        public Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, string ruleSet = null, CancellationToken cancellationToken = default) where TCommand : class, ICommand
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            var validator = this.serviceProvider.GetService<IAsyncObjectValidator<TCommand>>();
            if (validator == null) throw new InvalidOperationException($"The command validator for type {typeof(IAsyncObjectValidator<TCommand>)} could not be found.");
            return validator.ValidateAsync(command, ruleSet, cancellationToken);
        }

        #endregion Methods
    }
}