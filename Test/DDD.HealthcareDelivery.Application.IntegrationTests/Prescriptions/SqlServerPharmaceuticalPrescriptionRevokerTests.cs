using Xunit;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Domain.Prescriptions;
    using Infrastructure.Prescriptions;
    using Infrastructure;

    [Collection("SqlServer")]
    public class SqlServerPharmaceuticalPrescriptionRevokerTests
        : PharmaceuticalPrescriptionRevokerTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerPharmaceuticalPrescriptionRevokerTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IAsyncRepository<PharmaceuticalPrescription> CreateRepository()
        {
            return new PharmaceuticalPrescriptionRepository
            (
                new Domain.Prescriptions.BelgianPharmaceuticalPrescriptionTranslator(),
                new StoredEventTranslator(new GenericDataContractSerializer<IDomainEvent>()),
                new SqlServerHealthcareContextFactory(this.Fixture.ConnectionFactory)
            );
        }

        #endregion Methods

    }
}
