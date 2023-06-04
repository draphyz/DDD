namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a component that processes commands in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to implement a generic mechanism to consume events and manage recurring commands in the different bounded contexts.
    /// </remarks>
    public interface IContextualCommandProcessor<TContext> : IContextualCommandProcessor
        where TContext : BoundedContext
    {
        #region Properties

        /// <summary>
        /// The bounded context in which the command is processed.
        /// </summary>
        new TContext Context { get; }

        #endregion Properties

    }
}
