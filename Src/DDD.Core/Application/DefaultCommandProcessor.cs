using Conditions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// The command processor by default for processing and validating commands.  
    /// </summary>
    public class DefaultCommandProcessor : ICommandProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public DefaultCommandProcessor(IServiceProvider serviceProvider)
        {
            Condition.Requires(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public void Process<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<ISyncCommandHandler<TCommand>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(ISyncCommandHandler<TCommand>)} could not be found.");
            handler.Handle(command, context);
        }

        public Task ProcessAsync<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<IAsyncCommandHandler<TCommand>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(IAsyncCommandHandler<TCommand>)} could not be found.");
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