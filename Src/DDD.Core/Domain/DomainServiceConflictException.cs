using System;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Exception thrown when a conflict with other requests has been detected while calling a domain service.
    /// </summary>
    public class DomainServiceConflictException : DomainServiceException
    {

        #region Constructors

        public DomainServiceConflictException(Type serviceType = null, Exception innerException = null)
            : base(true, DefaultMessage(serviceType), serviceType, innerException)
        {
        }

        public DomainServiceConflictException(string message, Type serviceType = null, Exception innerException = null)
            : base(true, message, serviceType, innerException)
        {
        }

        #endregion Constructors

        #region Methods

        public static new string DefaultMessage(Type serviceType = null)
        {
            if (serviceType == null)
                return "A conflict has been detected while calling a domain service.";
            return $"A conflict has been detected while calling the domain service '{serviceType.Name}'.";
        }

        #endregion Methods

    }
}
