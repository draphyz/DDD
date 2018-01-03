namespace DDD.Core.Application
{
    using Validation; 

    /// <summary>
    /// Defines a method that validates a query of a specified type.
    /// </summary>
    public interface IQueryValidator<in TQuery> : IObjectValidator<TQuery>
        where TQuery : class, IQuery
    {
    }
}