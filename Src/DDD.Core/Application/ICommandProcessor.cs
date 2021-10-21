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

        void Process<TCommand>(TCommand command) where TCommand : class, ICommand;

        Task ProcessAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : class, ICommand;

        ValidationResult Validate<TCommand>(TCommand command, string ruleSet = null) where TCommand : class, ICommand;

        Task<ValidationResult> ValidateAsync<TCommand>(TCommand command, string ruleSet = null, CancellationToken cancellationToken = default) where TCommand : class, ICommand;

        #endregion Methods

    }
}
