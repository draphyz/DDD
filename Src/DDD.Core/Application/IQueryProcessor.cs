﻿using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Validation;

    /// <summary>
    /// Defines a component that validates and processes queries of any type.
    /// </summary>>
    public interface IQueryProcessor
    {

        #region Methods

        /// <summary>
        /// Specify the bounded context in which the query must be processed.
        /// </summary>
        IContextualQueryProcessor<TContext> In<TContext>(TContext context) where TContext : BoundedContext;

        /// <summary>
        /// Specify the bounded context in which the query must be processed.
        /// </summary>
        IContextualQueryProcessor In(BoundedContext context);

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