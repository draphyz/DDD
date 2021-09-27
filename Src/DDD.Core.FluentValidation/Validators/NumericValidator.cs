using FluentValidation;
using FluentValidation.Validators;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class NumericValidator<T> : PropertyValidator<T, string>
    {

        #region Properties

        public override string Name => "NumericValidator";

        #endregion Properties

        #region Methods

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value == null) return true;
            return value.IsNumeric();
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' should be numeric.";

        #endregion Methods

    }
}
