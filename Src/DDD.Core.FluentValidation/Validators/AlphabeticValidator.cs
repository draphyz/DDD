using FluentValidation;
using FluentValidation.Validators;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class AlphabeticValidator<T> : PropertyValidator<T, string>
    {

        #region Properties

        public override string Name => "AlphabeticValidator";

        #endregion Properties

        #region Methods

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value == null) return true;
            return value.IsAlphabetic();
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "'{PropertyName}' should be alphabetic.";

        #endregion Methods

    }
}
