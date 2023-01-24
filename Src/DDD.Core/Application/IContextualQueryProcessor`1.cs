using DDD.Core.Domain;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a component that processes generic queries in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to implement a generic mechanism to consume events and manage recurring commands in the different bounded contexts.
    /// </remarks>
    public interface IContextualQueryProcessor<TContext> : IContextualQueryProcessor
        where TContext : BoundedContext
    {

        #region Properties

        /// <summary>
        /// The bounded context in which the query is processed.
        /// </summary>
        new TContext Context { get; }

        #endregion Properties

    }
}
