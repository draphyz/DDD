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
            RevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Sent));
            NotRevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Revoked));
            NotRevocablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Delivered));
            DeliverablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Sent));
            NotDeliverablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Delivered));
            NotDeliverablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Revoked));
            NotDeliverablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Created));
            NotTransmittablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Sent));
            NotTransmittablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Delivered));
            NotTransmittablePrescriptions.Add(CreatePrescription(PrescriptionStatus.Revoked));
        }

        #endregion Constructors

        #region Properties

        public static TheoryData<ElectronicPharmaceuticalPrescription> NotTransmittablePrescriptions { get; } = new TheoryData<ElectronicPharmaceuticalPrescription>();

        #endregion Properties

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

        public override void Deliver_DeliverablePrescription_AddsPrescriptionDeliveredEvent(Prescription<PharmaceuticalPrescriptionState> prescription)
        {
            // Act
            prescription.Deliver();
            // Assert
            prescription.AllEvents().Should().ContainSingle(e => e is PharmaceuticalPrescriptionDelivered);
        }

        public override void Deliver_NotDeliverablePrescription_DoesNotAddEvent(Prescription<PharmaceuticalPrescriptionState> prescription)
        {
            // Act
            prescription.Deliver();
            // Assert
            prescription.AllEvents().Should().BeEmpty();
        }

        public override void Revoke_NotRevocablePrescription_DoesNotAddEvent(Prescription<PharmaceuticalPrescriptionState> prescription)
        {
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.AllEvents().Should().BeEmpty();
        }

        public override void Revoke_RevocablePrescription_AddsPrescriptionRevokedEvent(Prescription<PharmaceuticalPrescriptionState> prescription)
        {
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.AllEvents().Should().ContainSingle(e => e is PharmaceuticalPrescriptionRevoked);
        }

        [Theory]
        [MemberData(nameof(NotTransmittablePrescriptions))]
        public void Send_NotTransmittablePrescription_DoesNotAddEvent(ElectronicPharmaceuticalPrescription prescription)
        {
            // Act
            prescription.Send(new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6"));
            // Assert
            prescription.AllEvents().Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(NotTransmittablePrescriptions))]
        public void Send_NotTransmittablePrescription_DoesNotChangeStatus(ElectronicPharmaceuticalPrescription prescription)
        {
            // Arrange
            var initialStatus = prescription.ToState().Status;
            // Act
            prescription.Send(new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6"));
            // Assert
            var status = prescription.ToState().Status;
            status.Should().Be(initialStatus);
        }

        [Fact]
        public void Send_TransmittablePrescription_AddsPrescriptionSentEvent()
        {
            // Arrange
            var prescription = CreatePrescription(PrescriptionStatus.Created);
            // Act
            prescription.Send(new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6"));
            // Assert 
            prescription.AllEvents().Should().ContainSingle(e => e is PharmaceuticalPrescriptionSent);
        }

        [Fact]
        public void Send_TransmittablePrescription_MarksPrescriptionAsSent()
        {
            // Arrange
            var prescription = CreatePrescription(PrescriptionStatus.Created);
            // Act
            prescription.Send(new BelgianElectronicPrescriptionNumber("BEP1E4RFVHV6"));
            // Assert 
            var status = prescription.ToState().Status;
            status.Should().Be(PrescriptionStatus.Sent.Code);
        }

        private static ElectronicPharmaceuticalPrescription CreatePrescription(PrescriptionStatus status)
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
                new DateTime(2016, 2, 7)
            );
        }

        #endregion Methods

    }
}
