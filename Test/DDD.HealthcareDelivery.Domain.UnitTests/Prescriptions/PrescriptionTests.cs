using Xunit;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Infrastructure;

    [Trait("Category", "Unit")]
    public abstract class PrescriptionTests<TState> where TState : PrescriptionState, new()
    {

        #region Properties

        public static TheoryData<Prescription<TState>> DeliverablePrescriptions { get; } = new TheoryData<Prescription<TState>>();
        public static TheoryData<Prescription<TState>> NotDeliverablePrescriptions { get; } = new TheoryData<Prescription<TState>>();
        public static TheoryData<Prescription<TState>> NotRevocablePrescriptions { get; } = new TheoryData<Prescription<TState>>();
        public static TheoryData<Prescription<TState>> RevocablePrescriptions { get; } = new TheoryData<Prescription<TState>>();

        #endregion Properties

        #region Methods

        [Theory]
        [CustomMemberData(nameof(DeliverablePrescriptions))]
        public abstract void Deliver_DeliverablePrescription_AddsPrescriptionDeliveredEvent(Prescription<TState> prescription);

        [Theory]
        [CustomMemberData(nameof(DeliverablePrescriptions))]
        public void Deliver_DeliverablePrescription_MarksPrescriptionAsDelivered(Prescription<TState> prescription)
        {
            // Act
            prescription.Deliver();
            // Assert
            var status = prescription.ToState().Status;
            status.Should().Be(PrescriptionStatus.Delivered.Code);
        }

        [Theory]
        [CustomMemberData(nameof(NotDeliverablePrescriptions))]
        public abstract void Deliver_NotDeliverablePrescription_DoesNotAddEvent(Prescription<TState> prescription);

        [Theory]
        [CustomMemberData(nameof(NotDeliverablePrescriptions))]
        public void Deliver_NotDeliverablePrescription_DoesNotChangeStatus(Prescription<TState> prescription)
        {
            // Arrange
            var initialStatus = prescription.ToState().Status;
            // Act
            prescription.Deliver();
            // Assert
            var status = prescription.ToState().Status;
            status.Should().Be(initialStatus);
        }

        [Theory]
        [CustomMemberData(nameof(NotRevocablePrescriptions))]
        public abstract void Revoke_NotRevocablePrescription_DoesNotAddEvent(Prescription<TState> prescription);

        [Theory]
        [CustomMemberData(nameof(NotRevocablePrescriptions))]
        public void Revoke_NotRevocablePrescription_DoesNotChangeStatus(Prescription<TState> prescription)
        {
            // Arrange
            var initialStatus = prescription.ToState().Status;
            // Act
            prescription.Revoke("Erreur");
            // Assert
            var status = prescription.ToState().Status;
            status.Should().Be(initialStatus);
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
            var status = prescription.ToState().Status;
            status.Should().Be(PrescriptionStatus.Revoked.Code);
        }

        #endregion Methods

    }
}