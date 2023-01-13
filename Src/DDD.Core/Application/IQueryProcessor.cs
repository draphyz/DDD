using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a component that validates and processes queries of any type.
    /// </summary>>
    public interface IQueryProcessor
    {

        #region Methods

        /// <summary>
        /// Processes synchronously a query of a specified type and provides a result of a specified type.
        /// </summary>
        TResult Process<TResult>(IQuery<TResult> query, IMessageContext context = null);

        /// <summary>
        /// Processes asynchronously a query of a specified type and provides a result of a specified type.
        /// </summary>
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context = null);

        /// <summary>
        /// Validates synchronously a query of a specified type.
        /// </summary>
        ValidationResult Validate<TQuery>(TQuery query, string ruleSet = null) where TQuery : class, IQuery;

        /// <summary>
        /// Validates asynchronously a query of a specified type.
        /// </summary>
        Task<ValidationResult> ValidateAsync<TQuery>(TQuery query, string ruleSet = null, CancellationToken cancellationToken = default) where TQuery : class, IQuery;

        #endregion Methods

    }
}