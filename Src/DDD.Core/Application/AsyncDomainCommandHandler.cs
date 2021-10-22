using Conditions;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Mapping;
    using Threading;

    /// <summary>
    /// Base class for handling asynchronously commands using a domain model.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public abstract class AsyncDomainCommandHandler<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        #region Fields

        private readonly IObjectTranslator<DomainException, CommandException> exceptionTranslator = DomainToCommandExceptionTranslator.Default;

        #endregion Fields

        #region Methods

        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            await new SynchronizationContextRemover();
            try
            {
                await this.ExecuteAsync(command, cancellationToken);
            }
            catch (DomainException ex)
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        protected abstract Task ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);

        #endregion Methods

    }
}