using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class FluentCommandValidatorAdapter<TCommand> : FluentValidatorAdapter<TCommand>, ICommandValidator<TCommand>
        where TCommand : class, ICommand
    {

        #region Constructors

        public FluentCommandValidatorAdapter(IValidator<TCommand> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors

    }
}
