using Conditions;
using System;

namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Finds the correct command handler and validator and invokes them.  
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public CommandProcessor(IServiceProvider serviceProvider)
        {
            Condition.Requires(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public void Process<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var handler = this.serviceProvider.GetService<ICommandHandler<TCommand>>();
            if (handler == null) throw new InvalidOperationException($"The command handler for type {typeof(ICommandHandler<TCommand>)} could not be found.");
            handler.Handle(command);
        }

        public ValidationResult Validate<TCommand>(TCommand command, string ruleSet = null) where TCommand : class, ICommand
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var validator = this.serviceProvider.GetService<ICommandValidator<TCommand>>();
            if (validator == null) throw new InvalidOperationException($"The command validator for type {typeof(ICommandValidator<TCommand>)} could not be found.");
            return validator.Validate(command, ruleSet);
        }

        #endregion Methods

    }
}