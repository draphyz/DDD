using Conditions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// The command processor for processing and validating core generic commands associated with a specific bounded context. 
    /// </summary>
    public class CoreCommandProcessor<TContext> : ICommandProcessor<TContext>
        where TContext : class, IBoundedContext, new()
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public CoreCommandProcessor(IServiceProvider serviceProvider)
        {
            Condition.Requires(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
            this.Context = new TContext();
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        #endregion Properties

        #region Methods

        public void Process<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<ISyncCommandHandler<TCommand, TContext>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(ISyncCommandHandler<TCommand, TContext>)} could not be found.");
            handler.Handle(command, context);
        }

        public Task ProcessAsync<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<IAsyncCommandHandler<TCommand, TContext>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(IAsyncCommandHandler<TCommand, TContext>)} could not be found.");
            return handler.HandleAsync(command, context);
        }

        public ValidationResult Validate<TCommand>(TCommand command, string ruleSet = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var validator = this.serviceProvider.GetService<ISyncCommandValidator<TCommand>>();
            if (validator == null) throw new InvalidOperationException($"The command validator for type {typeof(ISyncCommandValidator<TCommand>)} could not be found.");
            return validator.Validate(command, ruleSet);
        }

        public Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, string ruleSet = null, CancellationToken cancellationToken = default) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var validator = this.serviceProvider.GetService<IAsyncCommandValidator<TCommand>>();
            if (validator == null) throw new InvalidOperationException($"The command validator for type {typeof(IAsyncCommandValidator<TCommand>)} could not be found.");
            return validator.ValidateAsync(command, ruleSet, cancellationToken);
        }

        #endregion Methods

    }
}