using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using System;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain;
    using Core.Domain;
    using Core.Infrastructure.Data;
    using Domain.Prescriptions;
    using Infrastructure;
    using Infrastructure.Prescriptions;

    public abstract class PharmaceuticalPrescriptionRevokerTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Fields

        private DbHealthcareDeliveryContext context;

        #endregion Fields

        #region Constructors

        protected PharmaceuticalPrescriptionRevokerTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.ConnectionProvider = fixture.CreateConnectionProvider(pooling:false); // To check transaction escalation (MSDTC)
            this.Repository = this.CreateRepository();
            this.Handler = new PharmaceuticalPrescriptionRevoker (Repository);
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<HealthcareDeliveryContext> ConnectionProvider { get; }

        protected PharmaceuticalPrescriptionRevoker Handler { get; }

        protected IRepository<PharmaceuticalPrescription, PrescriptionIdentifier> Repository { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
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

        private IRepository<PharmaceuticalPrescription, PrescriptionIdentifier> CreateRepository()
        {
            this.context = this.Fixture.CreateDbContext(this.ConnectionProvider);
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
