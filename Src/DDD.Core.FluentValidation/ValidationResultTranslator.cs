using FluentValidation.Results;
using FluentValidation;
using Conditions;
using System.Linq;
using System.Collections.Generic;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;

    internal class ValidationResultTranslator
        : IObjectTranslator<ValidationResult, DDD.Validation.ValidationResult>
    {

        #region Methods

        public DDD.Validation.ValidationResult Translate(ValidationResult result, IDictionary<string, object> options)
        {
            Condition.Requires(result, nameof(result)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("ObjectName"));
            var objectName = (string)options["ObjectName"];
            var isSuccessful = result.Errors.All(f => f.Severity == Severity.Info);
            var failures = result.Errors.Select(f => ToFailure(f)).ToArray();
            return new DDD.Validation.ValidationResult(isSuccessful, objectName, failures);
        }

        private static DDD.Validation.ValidationFailure ToFailure(ValidationFailure failure)
        {
            Condition.Requires(failure, nameof(failure)).IsNotNull();
            return new DDD.Validation.ValidationFailure
            (
                failure.ErrorMessage,
                failure.ErrorCode,
                failure.Severity.ToString().ToEnum<DDD.Validation.FailureLevel>(),
                failure.PropertyName,
                failure.AttemptedValue
            );
        }

        #endregion Methods

    }
}