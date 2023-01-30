using FluentValidation.Results;
using FluentValidation;
using EnsureThat;
using System.Linq;
using System.Collections.Generic;

namespace DDD.Core.Infrastructure.Validation
{
    using Mapping;

    internal class ValidationResultTranslator
        : ObjectTranslator<ValidationResult, DDD.Validation.ValidationResult>
    {

        #region Methods

        public override DDD.Validation.ValidationResult Translate(ValidationResult result, IDictionary<string, object> context = null)
        {
            Ensure.That(result, nameof(result)).IsNotNull();
            Ensure.That(context, nameof(context)).ContainsKey("ObjectName");
            var objectName = (string)context["ObjectName"];
            var isSuccessful = result.Errors.All(f => f.Severity == Severity.Info);
            var failures = result.Errors.Select(f => ToFailure(f)).ToArray();
            return new DDD.Validation.ValidationResult(isSuccessful, objectName, failures);
        }

        private static string GetCustomStateInfo(object customState, string infoType)
        {
            if (customState == null) return null;
            var state = customState.ToString();
            var parts = state.Split('=');
            if (parts.Length == 2 && parts[0] == infoType)
                return parts[1];
            return null;
        }

        private static DDD.Validation.ValidationFailure ToFailure(ValidationFailure failure)
        {
            Ensure.That(failure, nameof(failure)).IsNotNull();
            return new DDD.Validation.ValidationFailure
            (
                failure.ErrorMessage,
                failure.ErrorCode,
                failure.Severity.ToString().ToEnum<DDD.Validation.FailureLevel>(),
                failure.PropertyName,
                failure.AttemptedValue,
                GetCustomStateInfo(failure.CustomState, "category")
            );
        }

        #endregion Methods
    }
}
