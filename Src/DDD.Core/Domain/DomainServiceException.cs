using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a problem occurred while calling a domain service.
    /// </summary>
    public class DomainServiceException : DomainException
    {

        #region Constructors

        public DomainServiceException()
            : base("A problem occurred while calling a domain service.")
        {
        }

        public DomainServiceException(string message) : base(message)
        {
        }

        public DomainServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DomainServiceException(string message, Exception innerException, Type serviceType) : base(message, innerException)
        {
            this.ServiceType = serviceType;
        }

        public DomainServiceException(Exception innerException, Type serviceType)
            : base($"A problem occurred while calling the service '{serviceType.Name}'.", innerException)
        {
            this.ServiceType = serviceType;
        }

        #endregion Constructors

        #region Properties

        public Type ServiceType { get; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            var s = $"{this.GetType()}: {this.Message} ";
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
