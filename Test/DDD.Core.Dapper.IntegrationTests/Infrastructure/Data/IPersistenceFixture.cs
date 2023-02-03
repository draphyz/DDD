namespace DDD.Core.Infrastructure.Data
{
    using Core.Application;
    using Core.Domain;
    using Core.Infrastructure.Testing;
    using Domain;
    using Mapping;

    public interface IPersistenceFixture : IDbFixture<TestContext>
    {

        #region Methods

        IObjectTranslator<IEvent, Event> CreateEventTranslator();

        #endregion Methods

    }
}
