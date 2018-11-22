using FluentValidation.Validators;

namespace DDD.Core.Infrastructure.Validation.Validators
{
    using Core;

    internal class AlphanumericValidator : PropertyValidator
    {

        #region Constructors

        public AlphanumericValidator() : base("'{PropertyName}' should be alphanumeric.")
        {
        }

        #endregion Constructors

        #region Methods

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return true;
            var value = context.PropertyValue as string;
            if (value == null) return true;
            return value.IsAlphanumeric();
        }

        #endregion Methods

    }
}
