using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Core.Infrastructure.Data;
    using Domain.Prescriptions;
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

        protected override IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> CreateRepository()
        {
            var configuration = new BelgianSqlServerHealthcareConfiguration(SqlServerConnectionFactory.ConnectionString);
            var session = configuration.BuildSessionFactory().OpenSession();
            return new NHibernateRepository<PharmaceuticalPrescription, PrescriptionIdentifier>
             (
                session,
                new StoredEventTranslator(DataContractSerializerWrapper.Create(Encoding.Unicode))
            );
        }

        #endregion Methods

    }
}
