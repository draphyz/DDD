using FluentValidation;
using FluentValidation.Validators;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    internal class AlphanumericValidator<T> : PropertyValidator<T, string>
    {

        #region Properties

        public override string Name => "AlphanumericValidator";

        #endregion Properties

        #region Methods

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (value == null) return true;
            return value.IsAlphanumeric();
        }

        protected override string GetDefaultMessageTemplate(string errorCode) => "'{PropertyName}' should be alphanumeric.";

        #endregion Methods

    }
}
