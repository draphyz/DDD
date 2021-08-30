namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Domain;
    using Core.Infrastructure.Data;
    using Core.Infrastructure.Testing;
    using Mapping;

    public interface IPersistenceFixture<out TConnectionFactory> : IDbFixture<TConnectionFactory>
        where TConnectionFactory : class, IHealthcareDeliveryConnectionFactory
    {

        #region Methods

        HealthcareDeliveryContext CreateContext();
        IObjectTranslator<IEvent, StoredEvent> CreateEventTranslator();

        #endregion Methods

    }
}
