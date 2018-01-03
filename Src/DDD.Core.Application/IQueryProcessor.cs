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

        ValidationResult Validate<TQuery>(TQuery query, string ruleSet = null) where TQuery : class, IQuery;

        #endregion Methods

    }
}