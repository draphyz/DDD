using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a request to a domain service was denied.
    /// </summary>
    public class DomainServiceUnauthorizedException : DomainServiceException
    {

        #region Constructors

        public DomainServiceUnauthorizedException(Type serviceType = null, Exception innerException = null)
            : base(false, DefaultMessage(serviceType), serviceType, innerException)
        {
        }

        public DomainServiceUnauthorizedException(string message, Type serviceType = null, Exception innerException = null)
            : base(false, message, serviceType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type serviceType = null)
        {
            if (serviceType == null)
                return "The request to the domain service was denied.";
            return $"The request to the domain service '{serviceType.Name}' was denied.";
        }

        #endregion Methods

    }
}
