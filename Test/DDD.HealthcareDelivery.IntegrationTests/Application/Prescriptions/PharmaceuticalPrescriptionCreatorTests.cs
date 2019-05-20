using FluentAssertions;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Common.Application;
    using Core.Domain;
    using Core.Infrastructure.Testing;
    using Domain.Facilities;
    using Domain.Practitioners;
    using Domain.Prescriptions;
    using Infrastructure;

    public abstract class PharmaceuticalPrescriptionCreatorTests<TFixture>
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
    {

        #region Constructors

        protected PharmaceuticalPrescriptionCreatorTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.Repository = this.CreateRepository();
            this.Handler = new PharmaceuticalPrescriptionCreator
            (
                Repository,
                new EventPublisher(),
                new BelgianPharmaceuticalPrescriptionTranslator()
            );
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }
        protected PharmaceuticalPrescriptionCreator Handler { get; }
        protected IAsyncRepository<PharmaceuticalPrescription> Repository { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public async Task HandleAsync_WhenCalled_CreatePharmaceuticalPrescription()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("CreatePharmaceuticalPrescription");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("d.duck"), new string[] { "User" });
            var command = CreateCommand();
            // Act
            await this.Handler.HandleAsync(command);
            // Assert
            var prescription = await this.Repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier));
            prescription.Should().NotBeNull();
            prescription.Status.Should().Be(Domain.Prescriptions.PrescriptionStatus.Created);
            prescription.PrescribedMedications().Should().NotBeNullOrEmpty();
        }

        protected abstract IAsyncRepository<PharmaceuticalPrescription> CreateRepository();

        private static CreatePharmaceuticalPrescription CreateCommand()
        {
            return new CreatePharmaceuticalPrescription
            {
                LanguageCode = "fr",
                PrescriberIdentifier = 1,
                PrescriberLicenseNumber = "19006951001",
                PrescriberType = HealthcarePractitionerType.Physician,
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
                FacilityIdentifier = 1,
                FacilityType = HealthFacilityType.MedicalOffice,
                FacilityName = "Medical Office Donald Duck",
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
            };
        }

        #endregion Methods

    }
}
