using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class AsyncFluentCommandValidatorAdapter<TCommand> : AsyncFluentValidatorAdapter<TCommand>, IAsyncCommandValidator<TCommand>
        where TCommand : class, ICommand
    {

        #region Constructors

        public AsyncFluentCommandValidatorAdapter(IValidator<TCommand> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors

    }
}
