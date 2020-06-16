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
    using Infrastructure.Prescriptions;
    using Mapping;

    public abstract class PharmaceuticalPrescriptionCreatorTests<TFixture> : IDisposable
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
    {

        #region Fields

        private HealthcareContext context;

        #endregion Fields

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

        public void Dispose()
        {
            this.context.Dispose();
        }

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


        protected abstract HealthcareContext CreateContext();

        protected abstract IObjectTranslator<IEvent, EventState> CreateEventTranslator();

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
                DeliverableAt = new DateTime(2018, 2, 1),
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

        private IAsyncRepository<PharmaceuticalPrescription> CreateRepository()
        {
            this.context = this.CreateContext();
            return new PharmaceuticalPrescriptionRepository
            (
                this.context,
                new Domain.Prescriptions.BelgianPharmaceuticalPrescriptionTranslator(),
                this.CreateEventTranslator()
            );
        }

        #endregion Methods

    }
}
