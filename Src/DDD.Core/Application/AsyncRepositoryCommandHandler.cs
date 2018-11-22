using Conditions;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling asynchronously commands using the repository pattern.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <typeparam name="TDomainEntity">The type of the domain entity.</typeparam>
    public abstract class AsyncRepositoryCommandHandler<TCommand, TDomainEntity> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
        where TDomainEntity : DomainEntity

    {

        #region Constructors

        protected AsyncRepositoryCommandHandler(IAsyncRepository<TDomainEntity> repository,
                                                IEventPublisher publisher)
        {
            Condition.Requires(repository, nameof(repository)).IsNotNull();
            Condition.Requires(publisher, nameof(publisher)).IsNotNull();
            this.Repository = repository;
            this.Publisher = publisher;
        }

        #endregion Constructors

        #region Properties

        protected IEventPublisher Publisher { get; }

        protected IAsyncRepository<TDomainEntity> Repository { get; }

        #endregion Properties

        #region Methods

        public async Task HandleAsync(TCommand command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                await this.ExecuteAsync(command);
            }
            catch(RepositoryException ex)
            {
                throw new CommandException(ex, command);
            }
        }

        protected abstract Task ExecuteAsync(TCommand command);

        #endregion Methods

    }
}