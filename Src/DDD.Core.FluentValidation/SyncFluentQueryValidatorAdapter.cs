using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class SyncFluentQueryValidatorAdapter<TQuery> : SyncFluentValidatorAdapter<TQuery>, ISyncQueryValidator<TQuery>
        where TQuery : class, IQuery
    {
        #region Constructors

        public SyncFluentQueryValidatorAdapter(IValidator<TQuery> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors
    }
}
