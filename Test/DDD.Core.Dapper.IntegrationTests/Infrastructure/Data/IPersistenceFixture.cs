namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;
    using Testing;
    using Mapping;

    public interface IPersistenceFixture : IDbFixture<TestContext>
    {

        #region Methods

        IObjectTranslator<IEvent, Event> CreateEventTranslator();

        #endregion Methods

    }
}
