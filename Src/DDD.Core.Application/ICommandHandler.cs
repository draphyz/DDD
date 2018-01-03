namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that handles a command of a specified type.
    /// </summary>
    public interface ICommandHandler<in TCommand>
        where TCommand : class, ICommand
    {

        #region Methods

        void Handle(TCommand command);

        #endregion Methods

    }
}