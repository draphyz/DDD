using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that handles asynchronously a query of a specified type and provides a result of a specified type.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IAsyncQueryHandler<in TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        #region Methods

        /// <summary>
        /// Handles asynchronously a query of a specified type and provides a result of a specified type.
        /// </summary>
        Task<TResult> HandleAsync(TQuery query, IMessageContext context = null);

        #endregion Methods
    }
}