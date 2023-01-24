using DDD.Core.Domain;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that handles asynchronously a command of a specified type in a specific bounded context.
    /// </summary>
    public interface IAsyncCommandHandler<in TCommand, out TContext> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
        where TContext : BoundedContext
    {

        #region Properties

        /// <summary>
        /// The bounded context in which the command is handled.
        /// </summary>
        TContext Context { get; }

        #endregion Properties

    }
}