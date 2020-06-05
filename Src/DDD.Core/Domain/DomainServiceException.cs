using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when an error occurred while calling a domain service.
    /// </summary>
    public class DomainServiceException : DomainException
    {

        #region Constructors

        public DomainServiceException(bool isTransient, Type serviceType = null, Exception innerException = null) 
            : base(isTransient, DefaultMessage(serviceType), innerException)
        {
            this.ServiceType = serviceType;
        }

        public DomainServiceException(bool isTransient, string message, Type serviceType = null, Exception innerException = null) 
            : base(isTransient, message, innerException)
        {
            this.ServiceType = serviceType;
        }

        #endregion Constructors

        #region Properties

        public Type ServiceType { get; }

        #endregion Properties

        #region Methods

        public static string DefaultMessage(Type serviceType = null)
        {
            if (serviceType == null)
                return "An error occurred while calling a domain service.";
            return $"An error occurred while calling the service '{serviceType.Name}'.";
        }

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
            s += $"{Environment.NewLine}IsTransient: {this.IsTransient}";
            if (this.ServiceType != null)
                s += $"{Environment.NewLine}ServiceType: {this.ServiceType}";
            if (this.InnerException != null)
                s += $" ---> {this.InnerException}";
            if (this.StackTrace != null)
                s += $"{Environment.NewLine}{this.StackTrace}";
            return s;
        }

        #endregion Methods
    }
}
