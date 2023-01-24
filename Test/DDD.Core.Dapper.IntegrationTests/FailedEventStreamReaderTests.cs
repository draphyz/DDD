using System;

namespace DDD.Core.Infrastructure.Data
{
    public abstract class EventsByStreamIdFinderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected EventsByStreamIdFinderTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.ConnectionProvider = fixture.CreateConnectionProvider();
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<TestContext> ConnectionProvider { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
        }

        #endregion Methods

    }
}
