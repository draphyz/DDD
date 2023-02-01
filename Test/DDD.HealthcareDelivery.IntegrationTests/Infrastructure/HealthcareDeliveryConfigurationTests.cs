using FluentAssertions;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Application;
    using Common.Domain;
    using Domain.Patients;
    using Domain.Practitioners;
    using Domain.Prescriptions;

    public abstract class HealthcareDeliveryConfigurationTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Fields

        private readonly HealthcareDeliveryConfiguration configuration;
        private readonly ISessionFactory sessionFactory;
        private readonly ISession session;

        #endregion Fields

        #region Constructors

        protected HealthcareDeliveryConfigurationTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.configuration = this.CreateConfiguration();
            this.sessionFactory = this.configuration.BuildSessionFactory();
            this.session = this.sessionFactory.OpenSession();
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
            this.session.Dispose();
            this.sessionFactory.Dispose();
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
            Event event2;
            using (var transaction = this.session.BeginTransaction())
            {
                event2 = this.session.Get<Event>(event1.EventId);
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

        private static Event CreateEvent()
        {
            return new Event
            {
                EventId = new Guid("d9fdd908-9e0a-c80f-e72d-e94a0f7d4902"),
                EventType = "DDD.HealthcareDelivery.Domain.Prescriptions.PharmaceuticalPrescriptionCreated, DDD.HealthcareDelivery.Messages",
                OccurredOn = new DateTime(2018, 1, 1),
                Body = "{\"prescriptionId\":1,\"occurredOn\":\"2018 - 01 - 01T10:06:00\"}",
                BodyFormat = "JSON",
                StreamId = "1",
                StreamType = "PharmaceuticalPrescription",
                IssuedBy = "draphyz"
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