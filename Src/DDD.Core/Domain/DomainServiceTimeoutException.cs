using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a request to a domain service has expired.
    /// </summary>
    public class DomainServiceTimeoutException : DomainServiceException
    {

        #region Constructors

        public DomainServiceTimeoutException(Type serviceType = null, Exception innerException = null)
            : base(true, DefaultMessage(serviceType), serviceType, innerException)
        {
        }

        public DomainServiceTimeoutException(string message, Type serviceType = null, Exception innerException = null)
            : base(true, message, serviceType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type serviceType = null)
        {
            if (serviceType == null)
                return "The request to the domain service has expired.";
            return $"The request to the domain service '{serviceType.Name}' has expired.";
        }

        #endregion Methods

    }
}
