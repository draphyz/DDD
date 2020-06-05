using Conditions;

namespace DDD.Core.Application
{
    using Domain;
    using Mapping;

    /// <summary>
    /// Base class for handling synchronously commands using a domain model.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public abstract class DomainCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly IObjectTranslator<DomainException, CommandException> exceptionTranslator = DomainToCommandExceptionTranslator.Default;

        #endregion Fields

        #region Methods

        public void Handle(TCommand command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                this.Execute(command);
            }
            catch (DomainException ex)
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        protected abstract void Execute(TCommand command);

        #endregion Methods

    }

}
