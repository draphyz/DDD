namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles synchronously a query of a specified type and provides a result of a specified type in a specific bounded context.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public interface ISyncQueryHandler<in TQuery, out TResult, out TContext> : ISyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
        where TContext : BoundedContext
    {

        #region Properties

        /// <summary>
        /// The bounded context in which the query is handled.
        /// </summary>
        TContext Context { get; }

        #endregion Properties

    }
}