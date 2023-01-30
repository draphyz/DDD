using FluentValidation;
using FluentValidation.Results;
using EnsureThat;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;

    public class SyncFluentValidatorAdapter<T> : DDD.Validation.ISyncObjectValidator<T>
        where T : class
    {

        #region Fields

        private readonly IValidator<T> fluentValidator;
        private readonly IObjectTranslator<ValidationResult, DDD.Validation.ValidationResult> resultTranslator;

        #endregion Fields

        #region Constructors

        public SyncFluentValidatorAdapter(IValidator<T> fluentValidator)
        {
            Ensure.That(fluentValidator, nameof(fluentValidator)).IsNotNull();
            this.fluentValidator = fluentValidator;
            this.resultTranslator = new ValidationResultTranslator();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Validates synchronously the specified object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="ruleSet">The rule set.</param>
        public DDD.Validation.ValidationResult Validate(T obj, string ruleSet = null)
        {
            ValidationResult result;
            if (string.IsNullOrWhiteSpace(ruleSet))
                result = this.fluentValidator.Validate(obj);
            else
                result = this.fluentValidator.Validate(obj, context => context.IncludeRuleSets(ruleSet.Split(',')));
            return this.resultTranslator.Translate(result, new { ObjectName = obj.GetType().Name });
        }

        #endregion Methods

    }
}
