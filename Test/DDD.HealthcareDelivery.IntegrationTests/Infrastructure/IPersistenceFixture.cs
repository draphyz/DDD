namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Core.Domain;
    using Core.Infrastructure.Data;
    using Core.Infrastructure.Testing;
    using Core.Application;
    using Mapping;

    public interface IPersistenceFixture : IDbFixture<HealthcareDeliveryContext>
    {
        #region Methods

        DbHealthcareDeliveryContext CreateDbContext(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider);
        IObjectTranslator<IEvent, Event> CreateEventTranslator();

        #endregion Methods
    }
}
