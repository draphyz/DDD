using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class FluentQueryValidatorAdaptor<TQuery> : FluentValidatorAdaptor<TQuery>, IQueryValidator<TQuery>
        where TQuery : class, IQuery
    {
        #region Constructors

        public FluentQueryValidatorAdaptor(IValidator<TQuery> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors
    }
}
