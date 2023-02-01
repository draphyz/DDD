namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a method that validates synchronously a query of a specified type.
    /// </summary>
    public interface ISyncQueryValidator<in TQuery> : ISyncObjectValidator<TQuery>
        where TQuery : class, IQuery
    {
    }
}