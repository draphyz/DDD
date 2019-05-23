using Xunit;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Infrastructure.Testing;

    public abstract class PrescriptionTests
    {

        #region Properties

        public static TheoryData<Prescription> NotRevocablePrescriptions { get; } = new TheoryData<Prescription>();
        public static TheoryData<Prescription> RevocablePrescriptions { get; } = new TheoryData<Prescription>();

        #endregion Properties

        #region Methods

        [Theory]
        [CustomMemberData(nameof(NotRevocablePrescriptions))]
        public void Revoke_NotRevocablePrescription_DoesNotAddEvent(Prescription prescription)
        {
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.AllEvents().Should().BeEmpty();
        }

        [Theory]
        [CustomMemberData(nameof(NotRevocablePrescriptions))]
        public void Revoke_NotRevocablePrescription_DoesNotChangeStatus(Prescription prescription)
        {
            // Arrange
            var initialStatus = prescription.Status;
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.Status.Should().Be(initialStatus);
        }

        [Theory]
        [CustomMemberData(nameof(RevocablePrescriptions))]
        public abstract void Revoke_RevocablePrescription_AddsPrescriptionRevokedEvent(Prescription prescription);

        [Theory]
        [CustomMemberData(nameof(RevocablePrescriptions))]
        public void Revoke_RevocablePrescription_MarksPrescriptionAsRevoked(Prescription prescription)
        {
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.Status.Should().Be(PrescriptionStatus.Revoked);
        }

        #endregion Methods

    }
}