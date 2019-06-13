using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using Xunit;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Common.Domain;
    using Domain.Facilities;
    using Domain.Patients;
    using Domain.Practitioners;
    using Domain.Prescriptions;

    public abstract class HealthcareConfigurationTests
    {

        #region Fields

        private readonly HealthcareConfiguration configuration;
        private readonly ISession session;

        #endregion Fields

        #region Constructors

        protected HealthcareConfigurationTests()
        {
            this.configuration = this.CreateConfiguration();
            var sessionfactory = this.configuration.BuildSessionFactory();
            this.session = sessionfactory.OpenSession();
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void ExportSchema_WhenValidConfiguration_ThrowsNoException()
        {
            // Arrange
            var schemaExport = new SchemaExport(this.configuration);
            // Act
            schemaExport.Execute(useStdOut: true,
                                 execute: true,
                                 justDrop: false,
                                 connection: this.session.Connection,
                                 exportOutput: Console.Out);
            // Assert
        }

        [Fact]
        public void SavePrescription()
        {
            // Arrange
            var prescription1 = CreatePrescription();
            // Act
            using (var transaction = this.session.BeginTransaction())
            {
                this.session.Save(prescription1);
                transaction.Commit();
            }
            this.session.Clear();
            using (var transaction = this.session.BeginTransaction())
            {
                var prescription2 = this.session.Get<PharmaceuticalPrescription>(new PrescriptionIdentifier(1));
                transaction.Commit();
            }
            // Assert

        }

        protected abstract HealthcareConfiguration CreateConfiguration();

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
                new MedicalOffice
                (
                    1,
                    "Medical Office Donald Duck"
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