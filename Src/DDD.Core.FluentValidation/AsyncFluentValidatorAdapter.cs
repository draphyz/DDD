using FluentValidation;
using FluentValidation.Results;
using Conditions;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;
    using Threading;

    public class AsyncFluentValidatorAdapter<T> : DDD.Validation.IAsyncObjectValidator<T>
        where T : class
    {

        #region Fields

        private readonly IValidator<T> fluentValidator;
        private readonly IObjectTranslator<ValidationResult, DDD.Validation.ValidationResult> resultTranslator;

        #endregion Fields

        #region Constructors

        public AsyncFluentValidatorAdapter(IValidator<T> fluentValidator)
        {
            Condition.Requires(fluentValidator, nameof(fluentValidator)).IsNotNull();
            this.fluentValidator = fluentValidator;
            this.resultTranslator = new ValidationResultTranslator();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Validates asynchronously the specified object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        public async Task<DDD.Validation.ValidationResult> ValidateAsync(T obj, string ruleSet = null, CancellationToken cancellationToken = default)
        {
            await new SynchronizationContextRemover();
            ValidationResult result;
            if (string.IsNullOrWhiteSpace(ruleSet))
                result = await this.fluentValidator.ValidateAsync(obj, cancellationToken);
            else
                result = await this.fluentValidator.ValidateAsync(obj, context => context.IncludeRuleSets(ruleSet.Split(',')), cancellationToken);
            return this.resultTranslator.Translate(result, new { ObjectName = obj.GetType().Name });
        }

        #endregion Methods

    }
}
