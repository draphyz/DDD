using EnsureThat;
using System.Linq;

namespace DDD.Validation
{
    /// <summary>
    /// Represents a validation result.
    /// </summary>
    public class ValidationResult
    {

        #region Constructors

        public ValidationResult(bool isSuccessful, string objectName, ValidationFailure[] failures)
        {
            Ensure.That(objectName, nameof(objectName)).IsNotNullOrWhiteSpace();
            Ensure.That(failures, nameof(failures)).IsNotNull();
            this.IsSuccessful = isSuccessful;
            this.ObjectName = objectName;
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

        public string ObjectName { get; private set; }

        #endregion Properties

        #region Methods

        public bool HasFailures() => this.Failures.Any();

        #endregion Methods

    }
}
