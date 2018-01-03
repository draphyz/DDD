using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Validation
{
    /// <summary>
    /// Represents a validation result.
    /// </summary>
    public class ValidationResult
    {

        #region Constructors

        public ValidationResult(bool isSuccessful, IEnumerable<ValidationFailure> failures)
        {
            Condition.Requires(failures, nameof(failures)).IsNotNull();
            this.IsSuccessful = isSuccessful;
            this.Failures = failures;
        }

        private ValidationResult()
        {
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<ValidationFailure> Failures { get; private set; }

        public bool IsSuccessful { get; private set; }

        #endregion Properties

    }
}
