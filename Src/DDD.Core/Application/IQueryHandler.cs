namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that handles a query of a specified type and provides a result of a specified type.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IQueryHandler<in TQuery, out TResult>
        where TQuery : class, IQuery<TResult>
    {
        #region Methods

        TResult Handle(TQuery query);

        #endregion Methods
    }
}