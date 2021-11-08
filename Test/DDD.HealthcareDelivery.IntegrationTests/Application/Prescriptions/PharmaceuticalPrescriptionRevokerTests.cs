using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using System;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Domain.Prescriptions;
    using Infrastructure;
    using Infrastructure.Prescriptions;

    public abstract class PharmaceuticalPrescriptionRevokerTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture<IHealthcareDeliveryConnectionFactory>
    {

        #region Fields

        private HealthcareDeliveryContext context;

        #endregion Fields

        #region Constructors

        protected PharmaceuticalPrescriptionRevokerTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.Repository = this.CreateRepository();
            this.Handler = new PharmaceuticalPrescriptionRevoker
            (
                Repository
            );
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }
        protected PharmaceuticalPrescriptionRevoker Handler { get; }
        protected IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> Repository { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            this.context.Dispose();
        }

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

        private static RevokePharmaceuticalPrescription CreateCommand()
        {
            return new RevokePharmaceuticalPrescription
            {
                PrescriptionIdentifier = 1,
                RevocationReason = "Erreur"
            };
        }

        private IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> CreateRepository()
        {
            this.context = this.Fixture.CreateContext();
            return new PharmaceuticalPrescriptionRepository
            (
                this.context,
                new Domain.Prescriptions.BelgianPharmaceuticalPrescriptionTranslator(),
                this.Fixture.CreateEventTranslator()
            );
        }

        #endregion Methods

    }
}
