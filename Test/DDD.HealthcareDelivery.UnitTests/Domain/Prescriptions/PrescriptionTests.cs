using Xunit;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Infrastructure.Testing;

    public abstract class PrescriptionTests<TState> where TState : PrescriptionState, new()
    {

        #region Properties

        public static TheoryData<Prescription<TState>> NotRevocablePrescriptions { get; } = new TheoryData<Prescription<TState>>();
        public static TheoryData<Prescription<TState>> RevocablePrescriptions { get; } = new TheoryData<Prescription<TState>>();

        #endregion Properties

        #region Methods

        [Theory]
        [CustomMemberData(nameof(NotRevocablePrescriptions))]
        public void Revoke_NotRevocablePrescription_DoesNotAddEvent(Prescription<PharmaceuticalPrescriptionState> prescription)
        {
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.AllEvents().Should().BeEmpty();
        }

        [Theory]
        [CustomMemberData(nameof(NotRevocablePrescriptions))]
        public void Revoke_NotRevocablePrescription_DoesNotChangeStatus(Prescription<TState> prescription)
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
        public abstract void Revoke_RevocablePrescription_AddsPrescriptionRevokedEvent(Prescription<TState> prescription);

        [Theory]
        [CustomMemberData(nameof(RevocablePrescriptions))]
        public void Revoke_RevocablePrescription_MarksPrescriptionAsRevoked(Prescription<TState> prescription)
        {
            // Act
            prescription.Revoke("Erreur");
            // Assert
            prescription.Status.Should().Be(PrescriptionStatus.Revoked);
        }

        #endregion Methods

    }
}