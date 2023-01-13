namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that handles synchronously a command of a specified type in a specific bounded context.
    /// </summary>
    public interface ISyncCommandHandler<in TCommand, out TContext> : ISyncCommandHandler<TCommand>
        where TCommand : class, ICommand
        where TContext : class, IBoundedContext
    {

        #region Properties

        /// <summary>
        /// The bounded context in which the command is handled.
        /// </summary>
        TContext Context { get; }

        #endregion Properties

    }
}