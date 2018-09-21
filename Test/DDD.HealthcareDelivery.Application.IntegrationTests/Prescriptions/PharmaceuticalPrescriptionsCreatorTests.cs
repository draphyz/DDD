using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Linq;
using System;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Domain.Prescriptions;
    using Domain.Providers;
    using Domain.Facilities;
    using Common.Application;
    using Core.Infrastructure.Data;
    using Infrastructure;

    [Trait("Category", "Integration")]
    public abstract class PharmaceuticalPrescriptionsCreatorTests<TFixture>
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
    {

        #region Constructors

        protected PharmaceuticalPrescriptionsCreatorTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.Repository = this.CreateRepository();
            this.Handler = new PharmaceuticalPrescriptionsCreator
            (
                Repository,
                new DomainEventPublisher(),
                new BelgianPharmaceuticalPrescriptionTranslator()
            );
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }
        protected PharmaceuticalPrescriptionsCreator Handler { get; }
        protected IAsyncRepository<PharmaceuticalPrescription> Repository { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public async Task HandleAsync_WhenCalled_CreatePharmaceuticalPrescriptions()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("CreatePharmaceuticalPrescriptions");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("d.duck"), new string[] { "User" });
            var command = CreateCommand();
            // Act
            await this.Handler.HandleAsync(command);
            // Assert
            var prescription = (await this.Repository.FindAsync(new PrescriptionIdentifier(command.Prescriptions.First().PrescriptionIdentifier)))
                                                     .ToState();
            var medications = prescription.PrescribedMedications;
            prescription.Should().NotBeNull();
            prescription.Status.Should().Be(Domain.Prescriptions.PrescriptionStatus.Created.Code);
            medications.Should().NotBeNullOrEmpty();
        }

        protected abstract IAsyncRepository<PharmaceuticalPrescription> CreateRepository();

        private static CreatePharmaceuticalPrescriptions CreateCommand()
        {
            return new CreatePharmaceuticalPrescriptions
            {
                LanguageCode = "fr",
                PrescriberIdentifier = 1,
                PrescriberLicenseNumber = "19006951001",
                PrescriberType = HealthcareProviderType.Physician,
                PrescriberFirstName = "Donald",
                PrescriberLastName = "Duck",
                PrescriberDisplayName = "Dr. Duck Donald",
                PrescriberCountryCode = "BE",
                PrescriberPostalCode = "1000",
                PrescriberCity = "Brussel",
                PrescriberStreet = "Grote Markt",
                PrescriberHouseNumber = "7",
                PrescriberPrimaryTelephoneNumber = "02/221.21.21",
                PatientIdentifier = 1,
                PatientSocialSecurityNumber = "60207273601",
                PatientFirstName = "Fred",
                PatientLastName = "Flintstone",
                PatientBirthdate = new DateTime(1976, 2, 7),
                PatientSex = Sex.Male,
                HealthFacilityIdentifier = 1,
                HealthFacilityType = HealthFacilityType.Center,
                HealthFacilityName = "Healthcenter Donald Duck",
                Prescriptions = new PharmaceuticalPrescriptionDescriptor[]
                {
                    new PharmaceuticalPrescriptionDescriptor
                    {
                        PrescriptionIdentifier = 1,
                        CreatedOn = new DateTime(2018, 1, 1, 10, 6, 0),
                        DelivrableAt = new DateTime(2018, 2, 1),
                        Medications = new PrescribedMedicationDescriptor[]
                        {
                            new PrescribedMedicationDescriptor
                            {
                                MedicationType = PrescribedMedicationType.Product,
                                Code = "0318717",
                                NameOrDescription = "ADALAT OROS 30 COMP 28 X 30 MG",
                                Quantity = "1 boîte",
                                Posology = "appliquer 2 fois par jour jusqu'au 3 octobre 2018"
                            }
                        }
                    }
                },
            };
        }

        #endregion Methods

    }
}
