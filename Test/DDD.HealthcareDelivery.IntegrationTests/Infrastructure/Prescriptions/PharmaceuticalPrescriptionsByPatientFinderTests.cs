using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Testing;

    public abstract class PharmaceuticalPrescriptionsByPatientFinderTests<TFixture>
        where TFixture : IDbFixture<IHealthcareDeliveryConnectionFactory>
    {
        #region Fields

        private readonly TFixture fixture;

        #endregion Fields

        #region Constructors

        protected PharmaceuticalPrescriptionsByPatientFinderTests(TFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<object[]> QueriesAndResults()
        {
            yield return new object[]
            {
                new FindPharmaceuticalPrescriptionsByPatient { PatientIdentifier = 1 },
                new PharmaceuticalPrescriptionSummary[] { }
            };
            yield return new object[]
            {
                new FindPharmaceuticalPrescriptionsByPatient { PatientIdentifier = 12601 },
                new PharmaceuticalPrescriptionSummary[]
                {
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 1,
                        Status = PrescriptionStatus.Created,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DeliverableAt = null,
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 2,
                        Status = PrescriptionStatus.Created,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DeliverableAt = new DateTime(2017, 2, 18),
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 3,
                        Status = PrescriptionStatus.Created,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DeliverableAt = new DateTime(2017, 3, 18),
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 4,
                        Status = PrescriptionStatus.Created,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DeliverableAt = new DateTime(2017, 4, 18),
                        PrescriberDisplayName = "Dr. Duck Donald"
                    }

                }
            };
            yield return new object[]
            {
                new FindPharmaceuticalPrescriptionsByPatient { PatientIdentifier = 14314 },
                new PharmaceuticalPrescriptionSummary[]
                {
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 5,
                        Status = PrescriptionStatus.Created,
                        CreatedOn = new DateTime(2017, 9, 25),
                        DeliverableAt = null,
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 6,
                        Status = PrescriptionStatus.Created,
                        CreatedOn = new DateTime(2017, 9, 25),
                        DeliverableAt = new DateTime(2017, 12, 25),
                        PrescriberDisplayName = "Dr. Duck Donald"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults))]
        public async Task HandleAsync_WhenCalled_ReturnsValidResults(FindPharmaceuticalPrescriptionsByPatient query, IEnumerable<PharmaceuticalPrescriptionSummary> expectedResults)
        {
            // Arrange
            this.fixture.ExecuteScriptFromResources("FindPharmaceuticalPrescriptionsByPatient");
            var handler = new PharmaceuticalPrescriptionsByPatientFinder(this.fixture.ConnectionFactory);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, options => options.WithStrictOrdering());
        }

        #endregion Methods
    }
}