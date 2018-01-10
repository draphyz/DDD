using Xunit;
using System.Threading;
using System.Security.Principal;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Domain.Prescriptions;
    using Core.Infrastructure.Data;
    using Infrastructure;

    [Trait("Category", "Integration")]
    public abstract class PharmaceuticalPrescriptionRevokerTests<TFixture>
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
    {

        #region Constructors

        protected PharmaceuticalPrescriptionRevokerTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.Repository = this.CreateRepository();
            this.Handler = new PharmaceuticalPrescriptionRevoker
            (
                Repository,
                new DomainEventPublisher()
            );
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }
        protected PharmaceuticalPrescriptionRevoker Handler { get; }
        protected IRepository<PharmaceuticalPrescription> Repository { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public void Handle_WhenCalled_RevokePharmaceuticalPrescription()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("RevokePharmaceuticalPrescription");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("d.duck"), new string[] { "User" });
            var command = CreateCommand();
            // Act
            this.Handler.Handle(command);
            // Assert
            var prescription = this.Repository.Find(new PrescriptionIdentifier(command.PrescriptionIdentifier))
                                              .ToState();
            prescription.Status.Should().Be(Domain.Prescriptions.PrescriptionStatus.Revoked.Code);
        }

        protected abstract IRepository<PharmaceuticalPrescription> CreateRepository();

        private static RevokePharmaceuticalPrescription CreateCommand()
        {
            return new RevokePharmaceuticalPrescription
            {
                PrescriptionIdentifier = 1,
                RevocationReason = "Erreur"
            };
        }

        #endregion Methods

    }
}
