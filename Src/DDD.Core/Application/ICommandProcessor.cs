using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a component that validates and processes commands of any type.
    /// </summary>
    public interface ICommandProcessor
    {

        #region Methods

        /// <summary>
        /// Processes synchronously a command of a specified type.
        /// </summary>
        void Process<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand;

        /// <summary>
        /// Processes asynchronously a command of a specified type.
        /// </summary>
        Task ProcessAsync<TCommand>(TCommand command, IMessageContext context = null) where TCommand : class, ICommand;

        /// <summary>
        /// Validates synchronously a command of a specified type.
        /// </summary>
        ValidationResult Validate<TCommand>(TCommand command, string ruleSet = null) where TCommand : class, ICommand;

        /// <summary>
        /// Validates asynchronously a command of a specified type.
        /// </summary>
        Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, string ruleSet = null, CancellationToken cancellationToken = default) where TCommand : class, ICommand;

        #endregion Methods

    }
}
