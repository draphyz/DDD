namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    /// <summary>
    /// Defines a connection factory for the context of healthcare delivery.
    /// </summary>
    public interface IHealthcareDeliveryConnectionFactory : IDbConnectionFactory
    {
    }
}
