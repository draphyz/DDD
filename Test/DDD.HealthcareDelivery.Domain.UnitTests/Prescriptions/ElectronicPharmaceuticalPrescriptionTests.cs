using System;
using FluentAssertions;
using Xunit;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;
    using Patients;
    using Providers;
    using Facilities;

    public class ElectronicPharmaceuticalPrescriptionTests : PrescriptionTests<PharmaceuticalPrescriptionState>
    {

        #region Constructors

        static ElectronicPharmaceuticalPrescriptionTests()
        {
            RevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Created));
            NotRevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.InProcess));
            NotRevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Delivered));
            NotRevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Revoked));
            NotRevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Expired));
            NotRevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Archived));
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void Create_CreationDateNotSpecified_AddsPrescriptionCreatedEvent()
        {
            // Act
            var prescription = ElectronicPharmaceuticalPrescription.Create
                              (
                                  new PrescriptionIdentifier(1),
                                  new Physician(1, new FullName("Duck", "Donald"), new BelgianPractitionerLicenseNumber("19006951001")),
                                  new Patient(1, new FullName("Fred", "Flintstone"), BelgianSex.Male),
                                  new HealthcareCenter(1, "Healthcenter Donald Duck"),
                                  new PrescribedMedication[] { new PrescribedPharmaceuticalProduct("ADALAT OROS 30 COMP 28 X 30 MG", "appliquer 2 fois par jour") },
                                  new Alpha2LanguageCode("FR")
                              );
            // Assert
            prescription.AllEvents().Should().ContainSingle(e => e is PharmaceuticalPrescriptionCreated);
        }

        [Fact]
        public void Create_CreationDateNotSpecified_MarksPrescriptionAsCreated()
        {
            // Act
            var prescription = ElectronicPharmaceuticalPrescription.Create
                              (
                                  new PrescriptionIdentifier(1),
                                  new Physician(1, new FullName("Duck", "Donald"), new BelgianPractitionerLicenseNumber("19006951001")),
                                  new Patient(1, new FullName("Fred", "Flintstone"), BelgianSex.Male),
                                  new HealthcareCenter(1, "Healthcenter Donald Duck"),
                                  new PrescribedMedication[] { new PrescribedPharmaceuticalProduct("ADALAT OROS 30 COMP 28 X 30 MG", "appliquer 2 fois par jour") },
                                  new Alpha2LanguageCode("FR")
                              );
            // Assert
            var status = prescription.ToState().Status;
            status.Should().Be(PrescriptionStatus.Created.Code);
        }

        [Fact]
        public void Create_CreationDateSpecified_AddsPrescriptionCreatedEvent()
        {
            // Act
            var prescription = ElectronicPharmaceuticalPrescription.Create
                              (
                                  new PrescriptionIdentifier(1),
                                  new Physician(1, new FullName("Duck", "Donald"), new BelgianPractitionerLicenseNumber("19006951001")),
                                  new Patient(1, new FullName("Fred", "Flintstone"), BelgianSex.Male),
                                  new HealthcareCenter(1, "Healthcenter Donald Duck"),
                                  new PrescribedMedication[] { new PrescribedPharmaceuticalProduct("ADALAT OROS 30 COMP 28 X 30 MG", "appliquer 2 fois par jour") },
                                  new DateTime(2016, 2, 7),
                                  new Alpha2LanguageCode("FR")
                              );
            // Assert
            prescription.AllEvents().Should().ContainSingle(e => e is PharmaceuticalPrescriptionCreated);
        }

        [Fact]
        public void Create_CreationDateSpecified_MarksPrescriptionAsCreated()
        {
            // Act
            var prescription = ElectronicPharmaceuticalPrescription.Create
                              (
                                  new PrescriptionIdentifier(1),
                                  new Physician(1, new FullName("Duck", "Donald"), new BelgianPractitionerLicenseNumber("19006951001")),
                                  new Patient(1, new FullName("Fred", "Flintstone"), BelgianSex.Male),
                                  new HealthcareCenter(1, "Healthcenter Donald Duck"),
                                  new PrescribedMedication[] { new PrescribedPharmaceuticalProduct("ADALAT OROS 30 COMP 28 X 30 MG", "appliquer 2 fois par jour") },
                                  new DateTime(2016, 2, 7),
                                  new Alpha2LanguageCode("FR")
                              );
            // Assert
            var status = prescription.ToState().Status;
            status.Should().Be(PrescriptionStatus.Created.Code);
        }

        public override void Revoke_RevocablePrescription_AddsPrescriptionRevokedEvent(Prescription<PharmaceuticalPrescriptionState> prescription)
        {
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.AllEvents().Should().ContainSingle(e => e is PharmaceuticalPrescriptionRevoked);
        }

        [Fact]
        public void Send_NotSentPrescription_AddsPrescriptionSentEvent()
        {
            // Arrange
            var prescription = CreatePrescription(PrescriptionStatus.Created);
            // Act
            prescription.Send(new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6"));
            // Assert 
            prescription.AllEvents().Should().ContainSingle(e => e is PharmaceuticalPrescriptionSent);
        }

        [Fact]
        public void Send_NotSentPrescription_InitializesElectronicNumber()
        {
            // Arrange
            var prescription = CreatePrescription(PrescriptionStatus.Created);
            var specifiedNumber = new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6");
            // Act
            prescription.Send(specifiedNumber);
            // Assert 
            var number = prescription.ToState().ElectronicNumber;
            number.Should().Be(specifiedNumber.Number);
        }

        [Fact]
        public void Send_SentPrescription_DoesNotAddEvent()
        {
            // Arrange
            var prescription = CreatePrescription(PrescriptionStatus.Created, new BelgianElectronicPrescriptionNumber("BEP1LZS1LS97"));
            // Act
            prescription.Send(new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6"));
            // Assert
            prescription.AllEvents().Should().BeEmpty();
        }

        [Fact]
        public void Send_SentPrescription_DoesNotChangeElectronicNumber()
        {
            // Arrange
            var initialNumber = "BEP1LZS1LS97";
            var prescription = CreatePrescription(PrescriptionStatus.Created, new BelgianElectronicPrescriptionNumber(initialNumber));
            // Act
            prescription.Send(new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6"));
            // Assert
            var number = prescription.ToState().ElectronicNumber;
            number.Should().Be(initialNumber);
        }

        private static ElectronicPharmaceuticalPrescription CreatePrescription(PrescriptionStatus status, ElectronicPrescriptionNumber electronicNumber = null)
        {
            return new ElectronicPharmaceuticalPrescription
            (
                new PrescriptionIdentifier(1),
                new Physician(1, new FullName("Duck", "Donald"), new BelgianPractitionerLicenseNumber("19006951001")),
                new Patient(1, new FullName("Fred", "Flintstone"), BelgianSex.Male),
                new HealthcareCenter(1, "Healthcenter Donald Duck"),
                new PrescribedMedication[] { new PrescribedPharmaceuticalProduct("ADALAT OROS 30 COMP 28 X 30 MG", "appliquer 2 fois par jour") },
                new Alpha2LanguageCode("FR"),
                status,
                new DateTime(2016, 2, 7),
                null,
                electronicNumber
            );
        }

        #endregion Methods

    }
}
