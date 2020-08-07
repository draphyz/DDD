using System;
using FluentAssertions;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    public abstract class HealthcareDeliveryContextTests<TFixture>
        where TFixture : IPersistenceFixture<IHealthcareDeliveryConnectionFactory>
    {

        #region Fields

        private readonly TFixture fixture;

        #endregion Fields

        #region Constructors

        protected HealthcareDeliveryContextTests(TFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructors

        #region Methods

        [Fact(Skip = "This test recreates the database and thus can cause conflicts with other tests.")]
        public void CreateDatabase_DoesNotThrowException()
        {
            // Act
            Action createDatabase = () =>
            {
                using (var context = this.fixture.CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
            };
            // Assert
            createDatabase.Should().NotThrow();
        }

        #endregion Methods

    }
}
