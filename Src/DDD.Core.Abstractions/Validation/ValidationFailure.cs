using Conditions;

namespace DDD.Validation
{
    /// <summary>
    /// Represents a validation failure.
    /// </summary>
    public class ValidationFailure
    {

        #region Constructors

        public ValidationFailure(string message,
                                 string code,
                                 FailureLevel level = FailureLevel.Warning,
                                 string propertyName = null,
                                 object propertyValue = null)
        {
            Condition.Requires(message, nameof(message)).IsNotNullOrWhiteSpace();
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            this.Message = message;
            this.Code = code;
            this.Level = level;
            if (!string.IsNullOrWhiteSpace(propertyName))
                this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
        }

        /// <remarks>For serialization</remarks>
        private ValidationFailure()
        {
        }

        #endregion Constructors

        #region Properties

        public string Code { get; private set; }

        public FailureLevel Level { get; private set; }

        public string Message { get; private set; }

        public string PropertyName { get; private set; }

        public object PropertyValue { get; private set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{this.GetType().Name} [message={this.Message}, code={this.Code}, level={this.Level}, propertyName={this.PropertyName}, propertyValue={this.PropertyValue}]";
        }

        #endregion Methods

    }
}
