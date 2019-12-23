using Conditions;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Threading;

    /// <summary>
    /// Base class for handling asynchronously commands using a domain model.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public abstract class AsyncDomainCommandHandler<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Methods

        public async Task HandleAsync(TCommand command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            await new SynchronizationContextRemover();
            try
            {
                await this.ExecuteAsync(command);
            }
            catch (DomainException ex)
            {
                throw new CommandException(ex, command);
            }
        }

        protected abstract Task ExecuteAsync(TCommand command);

        #endregion Methods

    }
}