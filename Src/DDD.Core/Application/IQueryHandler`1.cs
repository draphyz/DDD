namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines methods that handle synchronously and asynchronously a query of a specified type and provides a result of a specified type in a specific bounded context.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public interface IQueryHandler<in TQuery, TResult, out TContext> : ISyncQueryHandler<TQuery, TResult, TContext>, IAsyncQueryHandler<TQuery, TResult, TContext>
        where TQuery : class, IQuery<TResult>
        where TContext : BoundedContext
    {
    }
}
