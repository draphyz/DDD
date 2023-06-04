namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that handles synchronously a command of a specified type.
    /// </summary>
    public interface ISyncCommandHandler<in TCommand>
        where TCommand : class, ICommand
    {

        #region Methods

        /// <summary>
        /// Handles synchronously a command of a specified type.
        /// </summary>
        void Handle(TCommand command, IMessageContext context);

        #endregion Methods

    }
}