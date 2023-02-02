using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a component that processes generic queries in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to implement a generic mechanism to consume events and manage recurring commands in the different bounded contexts.
    /// </remarks>
    public interface IContextualQueryProcessor
    {

        #region Properties

        /// <summary>
        /// The bounded context in which the query is processed.
        /// </summary>
        BoundedContext Context { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Processes synchronously a query of a specified type and provides a result of a specified type.
        /// </summary>
        TResult Process<TResult>(IQuery<TResult> query, IMessageContext context = null);

        /// <summary>
        /// Processes asynchronously a query of a specified type and provides a result of a specified type.
        /// </summary>
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context = null);

        #endregion Methods

    }
}
