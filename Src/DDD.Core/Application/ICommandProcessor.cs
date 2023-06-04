using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Validation;
    using Domain;

    /// <summary>
    /// Defines a component that validates and processes commands of any type.
    /// </summary>
    public interface ICommandProcessor
    {

        #region Methods

        /// <summary>
        /// Specify the bounded context in which the command must be processed.
        /// </summary>
        IContextualCommandProcessor<TContext> InGeneric<TContext>(TContext context) where TContext : BoundedContext;

        /// <summary>
        /// Specify the bounded context in which the command must be processed.
        /// </summary>
        IContextualCommandProcessor InSpecific(BoundedContext context);

        /// <summary>
        /// Processes synchronously a command of a specified type.
        /// </summary>
        void Process<TCommand>(TCommand command, IMessageContext context) where TCommand : class, ICommand;

        /// <summary>
        /// Processes asynchronously a command of a specified type.
        /// </summary>
        Task ProcessAsync<TCommand>(TCommand command, IMessageContext context) where TCommand : class, ICommand;

        /// <summary>
        /// Validates synchronously a command of a specified type.
        /// </summary>
        ValidationResult Validate<TCommand>(TCommand command, IValidationContext context) where TCommand : class, ICommand;

        /// <summary>
        /// Validates asynchronously a command of a specified type.
        /// </summary>
        Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, IValidationContext context) where TCommand : class, ICommand;

        #endregion Methods
    }
}
