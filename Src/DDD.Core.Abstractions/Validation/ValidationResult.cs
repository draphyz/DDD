using System.Collections.Generic;
using Conditions;

namespace DDD.Validation
{
    /// <summary>
    /// Represents a validation result.
    /// </summary>
    public class ValidationResult
    {

        #region Constructors

        public ValidationResult(bool isSuccessful, ValidationFailure[] failures)
        {
            Condition.Requires(failures, nameof(failures)).IsNotNull();
            this.IsSuccessful = isSuccessful;
            this.Failures = failures;
        }

        /// <remarks>For serialization</remarks>
        private ValidationResult()
        {
        }

        #endregion Constructors

        #region Properties

        public ValidationFailure[] Failures { get; private set; }

        public bool IsSuccessful { get; private set; }

        #endregion Properties

    }
}
