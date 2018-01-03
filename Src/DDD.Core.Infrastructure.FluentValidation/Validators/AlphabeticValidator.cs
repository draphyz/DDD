using FluentValidation.Validators;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    using Core;

    internal class AlphabeticValidator : PropertyValidator
    {

        #region Constructors

        public AlphabeticValidator() : base("'{PropertyName}' should be alphabetic.")
        {
        }

        #endregion Constructors

        #region Methods

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return true;
            var value = context.PropertyValue as string;
            if (value == null) return true;
            return value.IsAlphabetic();
        }

        #endregion Methods

    }
}
