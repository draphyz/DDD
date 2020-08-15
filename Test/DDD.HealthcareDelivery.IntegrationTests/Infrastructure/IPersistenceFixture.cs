using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Domain;
    using Core.Infrastructure.Testing;
    using Mapping;

    public interface IPersistenceFixture<out TConnectionFactory> : IDbFixture<TConnectionFactory>
        where TConnectionFactory : class, IHealthcareDeliveryConnectionFactory
    {

        #region Methods

        IObjectTranslator<IEvent, StoredEvent> CreateEventTranslator();

        ISessionFactory CreateSessionFactory();

        #endregion Methods
    }
}
