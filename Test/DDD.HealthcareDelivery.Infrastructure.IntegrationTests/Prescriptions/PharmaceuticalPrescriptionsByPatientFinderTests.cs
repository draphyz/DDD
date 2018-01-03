using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using System;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Data;

    [Trait("Category", "Integration")]
    public abstract class PharmaceuticalPrescriptionsByPatientFinderTests<TFixture>
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
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
                        Status = PrescriptionStatus.Sent,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DelivrableAt = null,
                        IsElectronic = true,
                        ElectronicNumber = "BEL27423142",
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 2,
                        Status = PrescriptionStatus.Sent,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DelivrableAt = new DateTime(2017, 2, 18),
                        IsElectronic = true,
                        ElectronicNumber = "BEL31802668",
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 3,
                        Status = PrescriptionStatus.Sent,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DelivrableAt = new DateTime(2017, 3, 18),
                        IsElectronic = true,
                        ElectronicNumber = "BEL95523055",
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 4,
                        Status = PrescriptionStatus.Sent,
                        CreatedOn = new DateTime(2016, 12, 18),
                        DelivrableAt = new DateTime(2017, 4, 18),
                        IsElectronic = true,
                        ElectronicNumber = "BEL65670306",
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
                        Status = PrescriptionStatus.Sent,
                        CreatedOn = new DateTime(2017, 9, 25),
                        DelivrableAt = null,
                        IsElectronic = true,
                        ElectronicNumber = "BEL98694269",
                        PrescriberDisplayName = "Dr. Duck Donald"
                    },
                    new PharmaceuticalPrescriptionSummary
                    {
                        Identifier = 6,
                        Status = PrescriptionStatus.Sent,
                        CreatedOn = new DateTime(2017, 9, 25),
                        DelivrableAt = new DateTime(2017, 12, 25),
                        IsElectronic = true,
                        ElectronicNumber = "BEL73420042",
                        PrescriberDisplayName = "Dr. Duck Donald"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults))]
        public void Handle_WhenCalled_ReturnsValidResults(FindPharmaceuticalPrescriptionsByPatient query, IEnumerable<PharmaceuticalPrescriptionSummary> expectedResults)
        {
            // Arrange
            this.fixture.ExecuteScriptFromResources("FindPharmaceuticalPrescriptionsByPatient");
            var handler = new PharmaceuticalPrescriptionsByPatientFinder(this.fixture.ConnectionFactory);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.ShouldAllBeEquivalentTo(expectedResults, options => options.WithStrictOrdering());
        }

        #endregion Methods
    }
}