namespace DDD.Core.Application
{
    /// <summary>
    /// Defines methods that handle synchronously and asynchronously a query of a specified type and provides a result of a specified type.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandler<in TQuery, TResult> : ISyncQueryHandler<TQuery, TResult>, IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
    }
}
