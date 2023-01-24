using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class SyncFluentCommandValidatorAdapter<TCommand> : SyncFluentValidatorAdapter<TCommand>, ISyncCommandValidator<TCommand>
        where TCommand : class, ICommand
    {

        #region Constructors

        public SyncFluentCommandValidatorAdapter(IValidator<TCommand> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors

    }
}
