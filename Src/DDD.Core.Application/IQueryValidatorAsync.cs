namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a method that validates asynchronously a query of a specified type.
    /// </summary>
    public interface IQueryValidatorAsync<in TQuery> : IObjectValidatorAsync<TQuery>
        where TQuery : class, IQuery
    {
    }
}