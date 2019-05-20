using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Domain.Prescriptions;
    using Core.Infrastructure.Testing;
    using Infrastructure;

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
                new EventPublisher()
            );
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }
        protected PharmaceuticalPrescriptionRevoker Handler { get; }
        protected IAsyncRepository<PharmaceuticalPrescription> Repository { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public async Task HandleAsync_WhenCalled_RevokePharmaceuticalPrescription()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("RevokePharmaceuticalPrescription");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("d.duck"), new string[] { "User" });
            var command = CreateCommand();
            // Act
            await this.Handler.HandleAsync(command);
            // Assert
            var prescription = await this.Repository.FindAsync(new PrescriptionIdentifier(command.PrescriptionIdentifier));
            prescription.Status.Should().Be(Domain.Prescriptions.PrescriptionStatus.Revoked);
        }

        protected abstract IAsyncRepository<PharmaceuticalPrescription> CreateRepository();

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
