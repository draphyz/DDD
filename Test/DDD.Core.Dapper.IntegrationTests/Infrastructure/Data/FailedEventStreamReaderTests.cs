using System;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public abstract class EventsByStreamIdFinderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected EventsByStreamIdFinderTests(TFixture fixture)
        {
            Fixture = fixture;
            ConnectionProvider = fixture.CreateConnectionProvider();
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<TestContext> ConnectionProvider { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            ConnectionProvider.Dispose();
        }

        #endregion Methods

    }
}
