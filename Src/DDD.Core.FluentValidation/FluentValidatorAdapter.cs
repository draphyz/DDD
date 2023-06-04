using System.Threading.Tasks;
using EnsureThat;
using FluentValidation;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;
    using Threading;
    using DDD.Validation;

    public class FluentValidatorAdapter<T> : IObjectValidator<T>
        where T : class
    {

        #region Fields

        private readonly IValidator<T> fluentValidator;
        private readonly IObjectTranslator<FluentValidation.Results.ValidationResult, ValidationResult> resultTranslator;

        #endregion Fields

        #region Constructors

        public FluentValidatorAdapter(IValidator<T> fluentValidator)
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
        /// <param name="context">The validation context.</param>
        public ValidationResult Validate(T obj, IValidationContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            FluentValidation.Results.ValidationResult result;
            var ruleSets = context.RuleSets();
            if (ruleSets == null)
                result = this.fluentValidator.Validate(obj);
            else
                result = this.fluentValidator.Validate(obj, c => c.IncludeRuleSets(ruleSets));
            return this.resultTranslator.Translate(result, new { ObjectName = obj.GetType().Name });
        }

        /// <summary>
        /// Validates asynchronously the specified object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="context">The validation context.</param>
        public async Task<ValidationResult> ValidateAsync(T obj, IValidationContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            await new SynchronizationContextRemover();
            FluentValidation.Results.ValidationResult result;
            var ruleSets = context.RuleSets();
            var cancellationToken = context.CancellationToken();
            if (ruleSets == null)
                result = await this.fluentValidator.ValidateAsync(obj, cancellationToken);
            else
                result = await this.fluentValidator.ValidateAsync(obj, c => c.IncludeRuleSets(ruleSets), cancellationToken);
            return this.resultTranslator.Translate(result, new { ObjectName = obj.GetType().Name });
        }

        #endregion Methods

    }
}
