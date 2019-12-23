using Xunit;
using System.Text;
using NHibernate;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Mapping;
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

        protected override IObjectTranslator<IEvent, StoredEvent> CreateEventTranslator()
        {
            return new StoredEventTranslator(DataContractSerializerWrapper.Create(Encoding.Unicode));
        }

        protected override ISession CreateSession()
        {
            var configuration = new BelgianSqlServerHealthcareConfiguration(SqlServerConnectionFactory.ConnectionString);
            return configuration.BuildSessionFactory().OpenSession();
        }

        #endregion Methods

    }
}
