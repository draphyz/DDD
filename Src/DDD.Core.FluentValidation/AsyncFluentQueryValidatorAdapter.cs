using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class AsyncFluentQueryValidatorAdapter<TQuery> : AsyncFluentValidatorAdapter<TQuery>, IAsyncQueryValidator<TQuery>
        where TQuery : class, IQuery
    {
        #region Constructors

        public AsyncFluentQueryValidatorAdapter(IValidator<TQuery> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors
    }
}
