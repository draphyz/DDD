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

        TResult Process<TResult>(IQuery<TResult> query);

        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

        ValidationResult Validate<TQuery>(TQuery query, string ruleSet = null) where TQuery : class, IQuery;

        Task<ValidationResult> ValidateAsync<TQuery>(TQuery query, string ruleSet = null, CancellationToken cancellationToken = default) where TQuery : class, IQuery;

        #endregion Methods

    }
}