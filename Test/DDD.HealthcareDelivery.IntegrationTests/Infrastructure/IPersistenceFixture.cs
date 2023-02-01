namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Core.Domain;
    using Core.Application;
    using Core.Infrastructure.Testing;
    using Core.Infrastructure.Data;
    using Mapping;

    public interface IPersistenceFixture : IDbFixture<HealthcareDeliveryContext>
    {

        #region Methods

        IObjectTranslator<IEvent, Event> CreateEventTranslator();

        DelegatingSessionFactory<HealthcareDeliveryContext> CreateSessionFactory(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider);

        #endregion Methods

    }
}
