using FluentValidation.Validators;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    using Core;

    internal class NumericValidator : PropertyValidator
    {

        #region Constructors

        public NumericValidator() : base("'{PropertyName}' should be numeric.")
        {
        }

        #endregion Constructors

        #region Methods

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return true;
            var value = context.PropertyValue as string;
            if (value == null) return true;
            return value.IsNumeric();
        }

        #endregion Methods

    }
}
