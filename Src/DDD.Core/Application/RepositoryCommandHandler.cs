using Conditions;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Base class for handling synchronously commands using the repository pattern.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <typeparam name="TDomainEntity">The type of the domain entity.</typeparam>
    public abstract class RepositoryCommandHandler<TCommand, TDomainEntity> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
        where TDomainEntity : DomainEntity

    {

        #region Constructors

        protected RepositoryCommandHandler(IRepository<TDomainEntity> repository,
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

        protected IRepository<TDomainEntity> Repository { get; }

        #endregion Properties

        #region Methods

        public void Handle(TCommand command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                this.Execute(command);
            }
            catch(RepositoryException ex)
            {
                throw new CommandException(ex, command);
            }
        }

        protected abstract void Execute(TCommand command);

        #endregion Methods

    }
}