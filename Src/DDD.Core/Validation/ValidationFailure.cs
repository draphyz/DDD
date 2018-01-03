using Conditions;

namespace DDD.Core.Validation
{
    /// <summary>
    /// Represents a validation failure.
    /// </summary>
    public class ValidationFailure
    {

        #region Constructors

        public ValidationFailure(string message, string code, FailureLevel level = FailureLevel.Warning)
        {
            Condition.Requires(message, nameof(message)).IsNotNullOrWhiteSpace();
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            this.Message = message;
            this.Code = code;
            this.Level = level;
        }

        private ValidationFailure()
        {
        }

        #endregion Constructors

        #region Properties

        public string Code { get; private set; }

        public FailureLevel Level { get; private set; }

        public string Message { get; private set; }

        #endregion Properties

    }
}
