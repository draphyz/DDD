using FluentAssertions;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Common.Domain;
    using Core.Infrastructure.Data;
    using Domain.Patients;
    using Domain.Practitioners;
    using Domain.Prescriptions;

    public abstract class HealthcareDeliveryConfigurationTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture<IHealthcareDeliveryConnectionFactory>
    {

        #region Fields

        private readonly HealthcareDeliveryConfiguration configuration;
        private readonly ISession session;

        #endregion Fields

        #region Constructors

        protected HealthcareDeliveryConfigurationTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.configuration = this.CreateConfiguration();
            var sessionfactory = this.configuration.BuildSessionFactory();
            this.session = sessionfactory.OpenSession();
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        #endregion Properties

        [Fact(Skip = "This test recreates the schema and thus can cause conflicts with other tests.")]
        public void HealthcareConfiguration_WhenMappingValid_CanExportSchema()
        {
            // Arrange
            var schemaExport = new SchemaExport(this.configuration);
            // Act
            Action action = () => schemaExport.Execute(useStdOut: true,
                                                       execute: true,
                                                       justDrop: false,
                                                       connection: this.session.Connection,
                                                       exportOutput: Console.Out);
            // Assert
            action.Should().NotThrow();
        }

        #region Methods

        public void Dispose()
        {
            this.session?.Dispose();
        }

        [Fact]
        public void HealthcareConfiguration_WhenMappingValid_CanSaveAndRestoreEvents()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("ClearDatabase");
            var event1 = CreateEvent();
            // Act
            using (var transaction = this.session.BeginTransaction())
            {
                this.session.Save(event1);
                transaction.Commit();
            }
            this.session.Clear();
            StoredEvent event2;
            using (var transaction = this.session.BeginTransaction())
            {
                event2 = this.session.Get<StoredEvent>(event1.Id);
                transaction.Commit();
            }
            // Assert
            event2.Should().BeEquivalentTo(event1);
        }

        [Fact]
        public void HealthcareConfiguration_WhenMappingValid_CanSaveAndRestorePrescriptions()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("ClearDatabase");
            var prescription1 = CreatePrescription();
            // Act
            using (var transaction = this.session.BeginTransaction())
            {
                this.session.Save(prescription1);
                transaction.Commit();
            }
            this.session.Clear();
            PharmaceuticalPrescription prescription2;
            using (var transaction = this.session.BeginTransaction())
            {
                prescription2 = this.session.Get<PharmaceuticalPrescription>(prescription1.Identifier);
                transaction.Commit();
            }
            // Assert
            prescription2.Should().BeEquivalentTo(prescription1);
        }
        protected abstract HealthcareDeliveryConfiguration CreateConfiguration();

        private static StoredEvent CreateEvent()
        {
            return new StoredEvent
            {
                Id = 1,
                Body = @"<PharmaceuticalPrescriptionCreated xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""DDD.HealthcareDelivery.Domain.Prescriptions""><PrescriptionId>1</PrescriptionId><OccurredOn>2018-01-01T10:06:00</OccurredOn></PharmaceuticalPrescriptionCreated>",
                StreamId = "1",
                UniqueId = Guid.NewGuid(),
                Username = "draphyz",
                EventType = "PharmaceuticalPrescriptionCreated",
                OccurredOn = new DateTime(2018, 1, 1)
            };
        }

        private static PharmaceuticalPrescription CreatePrescription()
        {
            return PharmaceuticalPrescription.Create
            (
                new PrescriptionIdentifier(1),
                new Physician
                (
                    1,
                    new FullName("Duck", "Donald"),
                    new BelgianHealthcarePractitionerLicenseNumber("19006951001"),
                    null,
                    new ContactInformation
                    (
                        new PostalAddress
                        (
                            "Grote Markt",
                            "Brussel",
                            "1000",
                            new Alpha2CountryCode("BE"),
                            "7"
                        ),
                        primaryTelephoneNumber: "02/221.21.21"
                    ),
                    displayName: "Dr. Duck Donald"
                ),
                new Patient
                (
                    1,
                    new FullName("Flintstone", "Fred"),
                    BelgianSex.Male,
                    new BelgianSocialSecurityNumber("60207273601")
                ),
                new PrescribedMedication[]
                {
                    new PrescribedPharmaceuticalProduct
                    (
                        nameOrDescription: "ADALAT OROS 30 COMP 28 X 30 MG",
                        posology: "appliquer 2 fois par jour jusqu'au 3 octobre 2018"
                    )
                },
                new Alpha2LanguageCode("FR")
            );
        }

        #endregion Methods

    }
}