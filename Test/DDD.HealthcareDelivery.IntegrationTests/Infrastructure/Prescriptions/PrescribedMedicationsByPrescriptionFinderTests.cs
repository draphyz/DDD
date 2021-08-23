using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Testing;

    public abstract class PrescribedMedicationsByPrescriptionFinderTests<TFixture>
        where TFixture : IDbFixture<IHealthcareDeliveryConnectionFactory>
    {
        #region Fields

        private readonly TFixture fixture;

        #endregion Fields

        #region Constructors

        protected PrescribedMedicationsByPrescriptionFinderTests(TFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructors

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
            this.fixture.ExecuteScriptFromResources("FindPrescribedMedicationsByPrescription");
            var handler = new PrescribedMedicationsByPrescriptionFinder(this.fixture.ConnectionFactory);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults);
        }

        #endregion Methods
    }
}