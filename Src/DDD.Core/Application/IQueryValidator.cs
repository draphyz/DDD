namespace DDD.Core.Application
{
    /// <summary>
    /// Defines methods that validate synchronously and asynchronously a query of a specified type.
    /// </summary>
    public interface IQueryValidator<in TQuery> : ISyncQueryValidator<TQuery>, IAsyncQueryValidator<TQuery>
        where TQuery : class, IQuery
    {
    }
}
