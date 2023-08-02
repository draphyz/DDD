using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Validation;
    using Domain;

    /// <summary>
    /// Defines a component that validates and processes queries of any type.
    /// </summary>>
    public interface IQueryProcessor
    {

        #region Methods

        /// <summary>
        /// Specify the bounded context in which the query must be processed.
        /// </summary>
        IContextualQueryProcessor<TContext> InGeneric<TContext>(TContext context) where TContext : BoundedContext;

        /// <summary>
        /// Specify the bounded context in which the query must be processed.
        /// </summary>
        IContextualQueryProcessor InSpecific(BoundedContext context);

        /// <summary>
        /// Processes synchronously a query of a specified type and provides a result of a specified type.
        /// </summary>
        TResult Process<TResult>(IQuery<TResult> query, IMessageContext context);

        /// <summary>
        /// Processes synchronously a query.
        /// </summary>
        object Process(IQuery query, IMessageContext context);

        /// <summary>
        /// Processes asynchronously a query of a specified type and provides a result of a specified type.
        /// </summary>
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context);

        /// <summary>
        /// Processes asynchronously a query.
        /// </summary>
        Task<object> ProcessAsync(IQuery query, IMessageContext context);

        /// <summary>
        /// Validates synchronously a query of a specified type.
        /// </summary>
        ValidationResult Validate<TQuery>(TQuery query, IValidationContext context) where TQuery : class, IQuery;

        /// <summary>
        /// Validates synchronously a query.
        /// </summary>
        ValidationResult Validate(IQuery query, IValidationContext context);

        /// <summary>
        /// Validates asynchronously a query of a specified type.
        /// </summary>
        Task<ValidationResult> ValidateAsync<TQuery>(TQuery query, IValidationContext context) where TQuery : class, IQuery;

        /// <summary>
        /// Validates asynchronously a query.
        /// </summary>
        Task<ValidationResult> ValidateAsync(IQuery query, IValidationContext context);

        #endregion Methods
    }
}