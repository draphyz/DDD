using System;
using System.Linq;
using System.Runtime.Serialization;

namespace DDD.Core.Domain
{
    using Validation;

    /// <summary>
    /// Exception thrown when a request to an domain service is invalid.
    /// </summary>
    public class DomainServiceInvalidException : DomainServiceException
    {

        #region Constructors

        public DomainServiceInvalidException(Type serviceType = null, ValidationFailure[] failures = null, Exception innerException = null)
            : base(false, DefaultMessage(serviceType), serviceType, innerException)
        {
            this.Failures = failures;
        }

        public DomainServiceInvalidException(string message, Type serviceType = null, ValidationFailure[] failures = null, Exception innerException = null)
            : base(false, message, serviceType, innerException)
        {
            this.Failures = failures;
        }

        #endregion Constructors

        #region Properties

        public ValidationFailure[] Failures { get; }

        #endregion Properties

        #region Methods

        public static new string DefaultMessage(Type serviceType = null)
        {
            if (serviceType == null)
                return "The request to the domain service is invalid.";
            return $"The request to the domain service '{serviceType.Name}' is invalid.";
        }

        public bool HasFailures() => this.Failures != null && this.Failures.Any();

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (this.Failures != null)
            {
                for (var i = 0; i < this.Failures.Length; i++)
                    info.AddValue($"Failure{i}", this.Failures[i]);
            }
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}Timestamp: {this.Timestamp}";
            s += $"{Environment.NewLine}IsTransient: {this.IsTransient}";
            if (this.ServiceType != null)
                s += $"{Environment.NewLine}ServiceType: {this.ServiceType}";
            if (this.Failures != null)
            {
                for (var i = 0; i < this.Failures.Length; i++)
                    s += $"{Environment.NewLine}Failure{i}: {this.Failures[i]}";
            }
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods

    }
}
