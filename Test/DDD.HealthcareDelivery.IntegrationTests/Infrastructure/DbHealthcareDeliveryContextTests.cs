using System;
using FluentAssertions;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Core.Infrastructure.Data;

    public abstract class DbHealthcareDeliveryContextTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Fields

        private DbHealthcareDeliveryContext context;

        #endregion Fields

        #region Constructors

        protected DbHealthcareDeliveryContextTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.ConnectionProvider = fixture.CreateConnectionProvider();
            this.context = this.Fixture.CreateDbContext(this.ConnectionProvider);
        }

        #endregion Constructors

        #region Properties

        protected IDbConnectionProvider<HealthcareDeliveryContext> ConnectionProvider { get; }

        protected TFixture Fixture { get; }

        #endregion Properties

        #region Methods

        [Fact(Skip = "This test recreates the database and thus can cause conflicts with other tests.")]
        public void CreateDatabase_DoesNotThrowException()
        {
            // Act
            Action createDatabase = () =>
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            };
            // Assert
            createDatabase.Should().NotThrow();
        }

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
            this.context.Dispose();
        }

        #endregion Methods

    }
}
