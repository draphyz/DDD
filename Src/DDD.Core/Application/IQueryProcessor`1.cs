namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a component that validates and processes generic core queries in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to implement a generic mechanism to consume events and manage recurring commands in the different bounded contexts.
    /// </remarks>
    public interface IQueryProcessor<TContext> : IQueryProcessor
        where TContext : class, IBoundedContext
    {

        #region Properties

        /// <summary>
        /// The bounded context in which the query is processed.
        /// </summary>
        TContext Context { get; }

        #endregion Properties

    }
}
