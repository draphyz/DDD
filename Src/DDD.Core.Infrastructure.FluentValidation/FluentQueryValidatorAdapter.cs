using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class FluentQueryValidatorAdapter<TQuery> : FluentValidatorAdapter<TQuery>, IQueryValidator<TQuery>
        where TQuery : class, IQuery
    {
        #region Constructors

        public FluentQueryValidatorAdapter(IValidator<TQuery> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors
    }
}
