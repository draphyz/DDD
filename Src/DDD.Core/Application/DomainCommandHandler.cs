using Conditions;

namespace DDD.Core.Application
{
    using Domain;
    using System;

    /// <summary>
    /// Base class for handling synchronously commands using a domain model.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public abstract class DomainCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Methods

        public void Handle(TCommand command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                this.Execute(command);
            }
            catch (Exception ex) when (ex is DomainServiceException || ex is RepositoryException)
            {
                throw new CommandException(ex, command);
            }
        }

        protected abstract void Execute(TCommand command);

        #endregion Methods

    }

}