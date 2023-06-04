using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using System;
using FluentAssertions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Application;
    using Domain;
    using Core.Infrastructure.Data;
    using Domain.Prescriptions;
    using Infrastructure;
    using Infrastructure.Prescriptions;

    public abstract class PharmaceuticalPrescriptionRevokerTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected PharmaceuticalPrescriptionRevokerTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.ConnectionProvider = fixture.CreateConnectionProvider(pooling: false); // To check transaction escalation (MSDTC)
            this.SessionFactory = this.Fixture.CreateSessionFactory(this.ConnectionProvider);
            this.Repository = this.CreateRepository();
            this.Handler = new PharmaceuticalPrescriptionRevoker(Repository);
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<HealthcareDeliveryContext> ConnectionProvider { get; }

        protected PharmaceuticalPrescriptionRevoker Handler { get; }

        protected PharmaceuticalPrescriptionRepository Repository { get; }

        protected DelegatingSessionFactory<HealthcareDeliveryContext> SessionFactory { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
            this.Repository.Dispose();
            this.SessionFactory.Dispose();
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_RevokePharmaceuticalPrescription()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("RevokePharmaceuticalPrescription");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("d.duck"), new string[] { "User" });
            var command = CreateCommand();
            var context = new MessageContext();
            // Act
            await this.Handler.HandleAsync(command, context);
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

        private PharmaceuticalPrescriptionRepository CreateRepository()
        {
            return new PharmaceuticalPrescriptionRepository
            (
                this.SessionFactory,
                this.Fixture.CreateEventTranslator()
            );
        }

        #endregion Methods

    }
}
