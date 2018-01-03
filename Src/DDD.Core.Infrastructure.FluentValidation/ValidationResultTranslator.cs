using FluentValidation.Results;
using FluentValidation;
using Conditions;
using System.Linq;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;

    internal class ValidationResultTranslator
        : IObjectTranslator<ValidationResult, Core.Validation.ValidationResult>
    {

        #region Methods

        public Core.Validation.ValidationResult Translate(ValidationResult result)
        {
            Condition.Requires(result, nameof(result)).IsNotNull();
            var isSuccessful = result.Errors.All(f => f.Severity == Severity.Info);
            var failures = result.Errors.Select(f => ToFailure(f));
            return new Core.Validation.ValidationResult(isSuccessful, failures);
        }

        private static Core.Validation.ValidationFailure ToFailure(ValidationFailure failure)
        {
            Condition.Requires(failure, nameof(failure)).IsNotNull();
            return new Core.Validation.ValidationFailure
            (
                $"{failure.ErrorMessage} (property: {failure.PropertyName}, value: {failure.AttemptedValue})",
                failure.ErrorCode,
                failure.Severity.ToString().ToEnum<Core.Validation.FailureLevel>()
            );
        }

        #endregion Methods

    }
}
