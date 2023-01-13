namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a component that validates and processes generic core commands in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to implement a generic mechanism to consume events and manage recurring commands in the different bounded contexts.
    /// </remarks>
    public interface ICommandProcessor<TContext> : ICommandProcessor
        where TContext : class, IBoundedContext
    {
        #region Properties

        /// <summary>
        /// The bounded context in which the command is processed.
        /// </summary>
        TContext Context { get; }

        #endregion Properties
    }
}
