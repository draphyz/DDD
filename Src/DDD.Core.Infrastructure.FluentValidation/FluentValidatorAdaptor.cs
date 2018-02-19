using FluentValidation;
using FluentValidation.Results;
using Conditions;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;

    public class FluentValidatorAdaptor<T> 
        : Core.Validation.IObjectValidator<T>, Core.Validation.IAsyncObjectValidator<T>
        where T : class
    {

        #region Fields

        private readonly IObjectTranslator<ValidationResult, Core.Validation.ValidationResult> resultTranslator;
        private readonly IValidator<T> fluentValidator;

        #endregion Fields

        public FluentValidatorAdaptor(IValidator<T> fluentValidator)
        {
            Condition.Requires(fluentValidator, nameof(fluentValidator)).IsNotNull();
            this.fluentValidator = fluentValidator;
            this.resultTranslator = new ValidationResultTranslator();
        }

        #region Methods

        /// <summary>
        /// Validates the specified object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="ruleSet">The rule set.</param>
        public Core.Validation.ValidationResult Validate(T obj, string ruleSet = null)
        {
            var result = this.fluentValidator.Validate(obj, ruleSet: ruleSet);
            return this.resultTranslator.Translate(result);
        }

        /// <summary>
        /// Validates asynchronously the specified object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="ruleSet">The rule set.</param>
        public async Task<Core.Validation.ValidationResult> ValidateAsync(T obj, string ruleSet = null)
        {
            var result = await this.fluentValidator.ValidateAsync(obj, ruleSet: ruleSet);
            return this.resultTranslator.Translate(result);
        }

        #endregion Methods

    }
}
