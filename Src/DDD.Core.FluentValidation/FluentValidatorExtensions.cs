using FluentValidation;
using System.Collections;

namespace DDD.Core.Infrastructure.Validation
{
    using Validators;

    public static class FluentValidatorExtensions
    {
        #region Methods

        /// <summary>
        /// Defines an alphabetic validator for strings on the current rule builder.
        /// Validation will fail if the string is not alphabetic.
        /// </summary>
        public static IRuleBuilderOptions<T, string> Alphabetic<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new AlphabeticValidator<T>());
        }

        /// <summary>
        /// Defines an alphanumeric validator for strings on the current rule builder.
        /// Validation will fail if the string is not alphanumeric.
        /// </summary>
        public static IRuleBuilderOptions<T, string> Alphanumeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new AlphanumericValidator<T>());
        }

        /// <summary>
        /// Defines a count validator for collections on the current rule builder.
        /// Validation will fail if the count of items in the collection is outside of the specifed range. The range is inclusive.
        /// </summary>
        public static IRuleBuilderOptions<T, IEnumerable> Count<T>(this IRuleBuilder<T, IEnumerable> ruleBuilder, int min, int max)
        {
            return ruleBuilder.SetValidator(new CountValidator<T>(min, max));
        }

        /// <summary>
        /// Defines a count validator for collections on the current rule builder.
        /// Validation will fail if the count of items in the collection is not equal to the specified count.
        /// </summary>
        public static IRuleBuilderOptions<T, IEnumerable> Count<T>(this IRuleBuilder<T, IEnumerable> ruleBuilder, int count)
        {
            return ruleBuilder.SetValidator(new ExactCountValidator<T>(count));
        }

        /// <summary>
        /// Defines a count validator for collections on the current rule builder.
        /// Validation will fail if the count of items in the collection is greater than the specified count.
        /// </summary>
        public static IRuleBuilderOptions<T, IEnumerable> MaximumCount<T>(this IRuleBuilder<T, IEnumerable> ruleBuilder, int max)
        {
            return ruleBuilder.SetValidator(new MaximumCountValidator<T>(max));
        }

        /// <summary>
        /// Defines a count validator for collections on the current rule builder.
        /// Validation will fail if the count of items in the collection is less than the specified count.
        /// </summary>
        public static IRuleBuilderOptions<T, IEnumerable> MinimumCount<T>(this IRuleBuilder<T, IEnumerable> ruleBuilder, int min)
        {
            return ruleBuilder.SetValidator(new MinimumCountValidator<T>(min));
        }

        /// <summary>
        /// Defines a numeric validator for strings on the current rule builder.
        /// Validation will fail if the string is not numeric.
        /// </summary>
        public static IRuleBuilderOptions<T, string> Numeric<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NumericValidator<T>());
        }

        #endregion Methods

    }
}
