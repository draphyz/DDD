using Xunit;
using System.Data.Common;
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
    using Infrastructure;

    public abstract class PharmaceuticalPrescriptionRevokerTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture<IHealthcareDeliveryConnectionFactory>
    {

        #region Fields

        private ISessionFactory sessionFactory;
        private ISession session;
        private DbConnection connection;

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
            this.session.Dispose();
            this.connection.Dispose();
            this.sessionFactory.Dispose();
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
            this.sessionFactory = this.Fixture.CreateSessionFactory();
            this.connection = this.Fixture.ConnectionFactory.CreateOpenConnection();
            this.session = this.sessionFactory
                .WithOptions()
                // To avoid transaction promotion from local to distributed
                .Connection(this.connection)
                .OpenSession();
            return new NHibernateRepository<PharmaceuticalPrescription, PrescriptionIdentifier>
             (
                this.session,
                this.Fixture.CreateEventTranslator()
            );
        }

        #endregion Methods

    }
}
