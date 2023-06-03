using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Application;
    using Domain;
    using Application.Prescriptions;
    using Core.Infrastructure.Testing;
    using Core.Infrastructure.Data;

    public abstract class PrescribedMedicationsByPrescriptionFinderTests<TFixture> : IDisposable
        where TFixture : IDbFixture<HealthcareDeliveryContext>
    {

        #region Constructors

        protected PrescribedMedicationsByPrescriptionFinderTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.ConnectionProvider = fixture.CreateConnectionProvider();
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<HealthcareDeliveryContext> ConnectionProvider { get; }

        #endregion Properties

        #region Methods

        public static IEnumerable<object[]> QueriesAndResults()
        {
            yield return new object[]
            {
                new FindPrescribedMedicationsByPrescription { PrescriptionIdentifier = 100 },
                new PrescribedMedicationDetails[] { }
            };
            yield return new object[]
            {
                new FindPrescribedMedicationsByPrescription { PrescriptionIdentifier = 1 },
                new PrescribedMedicationDetails[]
                {
                    new PrescribedMedicationDetails
                    {
                        Identifier = 2,
                        NameOrDescription = "Dualkopt Coll. 10 ml",
                        Posology = "1 goutte 2 x/jour",
                        Quantity = 1,
                        Code = "3260072"
                    },
                    new PrescribedMedicationDetails
                    {
                        Identifier = 1,
                        NameOrDescription = "Latansoc Mylan Coll. 2,5 ml X 3",
                        Posology = "1 goutte le soir",
                        Quantity = 1,
                        Code = null
                    }
                }
            };
            yield return new object[]
            {
                new FindPrescribedMedicationsByPrescription { PrescriptionIdentifier = 2 },
                new PrescribedMedicationDetails[]
                {
                    new PrescribedMedicationDetails
                    {
                        Identifier = 3,
                        NameOrDescription = "Dualkopt Coll. 10 ml",
                        Posology = "1 goutte 2 x/jour",
                        Quantity = 1,
                        Code = "3260072"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults))]
        public async Task HandleAsync_WhenCalled_ReturnsValidResults(FindPrescribedMedicationsByPrescription query, IEnumerable<PrescribedMedicationDetails> expectedResults)
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("FindPrescribedMedicationsByPrescription");
            var handler = new PrescribedMedicationsByPrescriptionFinder(this.ConnectionProvider);
            var context = new MessageContext();
            // Act
            var results = await handler.HandleAsync(query, context);
            // Assert
            results.Should().BeEquivalentTo(expectedResults);
        }

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
        }

        #endregion Methods
    }
}