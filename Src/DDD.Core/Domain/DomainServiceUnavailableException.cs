using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a domain service is currently unavailable.
    /// </summary>
    public class DomainServiceUnavailableException : DomainServiceException
    {

        #region Constructors

        public DomainServiceUnavailableException(Type serviceType = null, Exception innerException = null)
            : base(true, DefaultMessage(serviceType), serviceType, innerException)
        {
        }

        public DomainServiceUnavailableException(string message, Type serviceType = null, Exception innerException = null)
            : base(true, message, serviceType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type serviceType = null)
        {
            if (serviceType == null)
                return "The domain service is currently unavailable.";
            return $"The domain service '{serviceType.Name}' is currently unavailable.";
        }

        #endregion Methods

    }
}
