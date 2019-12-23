using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Core.Infrastructure.Data;
    using Domain.Prescriptions;
    using Infrastructure;
    using DDD.Mapping;
    using NHibernate;

    [Collection("SqlServer")]
    public class SqlServerPharmaceuticalPrescriptionCreatorTests
        : PharmaceuticalPrescriptionCreatorTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerPharmaceuticalPrescriptionCreatorTests(SqlServerFixture fixture) : base(fixture)
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
