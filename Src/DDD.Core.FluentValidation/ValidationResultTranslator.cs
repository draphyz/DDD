using FluentValidation.Results;
using FluentValidation;
using Conditions;
using System.Linq;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;

    internal class ValidationResultTranslator
        : IObjectTranslator<ValidationResult, DDD.Validation.ValidationResult>
    {

        #region Methods

        public DDD.Validation.ValidationResult Translate(ValidationResult result)
        {
            Condition.Requires(result, nameof(result)).IsNotNull();
            var isSuccessful = result.Errors.All(f => f.Severity == Severity.Info);
            var failures = result.Errors.Select(f => ToFailure(f));
            return new DDD.Validation.ValidationResult(isSuccessful, failures);
        }

        private static DDD.Validation.ValidationFailure ToFailure(ValidationFailure failure)
        {
            Condition.Requires(failure, nameof(failure)).IsNotNull();
            return new DDD.Validation.ValidationFailure
            (
                $"{failure.ErrorMessage} (property: {failure.PropertyName}, value: {failure.AttemptedValue})",
                failure.ErrorCode,
                failure.Severity.ToString().ToEnum<DDD.Validation.FailureLevel>()
            );
        }

        #endregion Methods

    }
}
