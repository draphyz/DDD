using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Application;

    public class FluentCommandValidatorAdaptor<TCommand> : FluentValidatorAdaptor<TCommand>, ICommandValidator<TCommand>
        where TCommand : class, ICommand
    {

        #region Constructors

        public FluentCommandValidatorAdaptor(IValidator<TCommand> fluentValidator) : base(fluentValidator)
        {
        }

        #endregion Constructors

    }
}
