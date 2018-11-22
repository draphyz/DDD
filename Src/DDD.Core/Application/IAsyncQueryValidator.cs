namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a method that validates asynchronously a query of a specified type.
    /// </summary>
    public interface IAsyncQueryValidator<in TQuery> : IAsyncObjectValidator<TQuery>
        where TQuery : class, IQuery
    {
    }
}