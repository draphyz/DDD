using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using FluentAssertions;
using NHibernate;
using System;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Data;
    using Domain.Prescriptions;
    using Core.Infrastructure.Testing;
    using Infrastructure;
    using Mapping;

    public abstract class PharmaceuticalPrescriptionRevokerTests<TFixture> : IDisposable
        where TFixture : IDbFixture<IHealthcareConnectionFactory>
    {

        #region Fields

        private ISession session;

        #endregion Fields

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
        protected IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> Repository { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            this.session.Dispose();
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

        protected abstract IObjectTranslator<IEvent, StoredEvent> CreateEventTranslator();

        protected abstract ISession CreateSession();

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
            this.session = this.CreateSession();
            return new NHibernateRepository<PharmaceuticalPrescription, PrescriptionIdentifier>
             (
                this.session,
                this.CreateEventTranslator()
            );
        }

        #endregion Methods

    }
}
