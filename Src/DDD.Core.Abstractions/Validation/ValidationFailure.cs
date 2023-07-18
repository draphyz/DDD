using EnsureThat;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DDD.Validation
{
    /// <summary>
    /// Represents a validation failure.
    /// </summary>
    [DataContract()]
    [XmlType()]
    public class ValidationFailure
    {

        #region Constructors

        public ValidationFailure(string message,
                                 string code,
                                 FailureLevel level = FailureLevel.Warning,
                                 string propertyName = null,
                                 object propertyValue = null,
                                 string category = null)
        {
            Ensure.That(message, nameof(message)).IsNotNullOrWhiteSpace();
            Ensure.That(code, nameof(code)).IsNotNullOrWhiteSpace();
            this.Message = message;
            this.Code = code;
            this.Level = level;
            if (!string.IsNullOrWhiteSpace(propertyName))
                this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
            if (!string.IsNullOrWhiteSpace(category))
                this.Category = category;
        }

        /// <remarks>For serialization</remarks>
        private ValidationFailure()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The category of the failure.
        /// </summary>
        [DataMember(Name = "category")]
        [XmlElement("category")]
        public string Category { get; private set; }

        /// <summary>
        /// A code associated with the failure.
        /// </summary>
        [DataMember(Name = "code")]
        [XmlElement("code")]
        public string Code { get; private set; }

        /// <summary>
        /// The level of the failure.
        /// </summary>
        [DataMember(Name = "level")]
        [XmlElement("level")]
        public FailureLevel Level { get; private set; }

        /// <summary>
        /// The failure message.
        /// </summary>
        [DataMember(Name = "message")]
        [XmlElement("message")]
        public string Message { get; private set; }

        /// <summary>
		/// The name of the property associated with the failure.
		/// </summary>
        [DataMember(Name = "propertyName")]
        [XmlElement("propertyName")]
        public string PropertyName { get; private set; }

        /// <summary>
		/// The property value that caused the failure.
		/// </summary>
        [DataMember(Name = "propertyValue")]
        [XmlElement("propertyValue")]
        public object PropertyValue { get; private set; }

        #endregion Properties

        #region Methods

        public override string ToString() 
            => $"{GetType().Name} [{nameof(Message)}={Message}, {nameof(Code)}={Code}, {nameof(Level)}={Level}, {nameof(PropertyName)}={PropertyName}, {nameof(PropertyValue)}={PropertyValue}, {nameof(Category)}={Category}]";

        #endregion Methods

    }
}
