using Xunit;
using System.Text;
using NHibernate;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Infrastructure;
    using Mapping;

    [Collection("Oracle")]
    public class OraclePharmaceuticalPrescriptionRevokerTests
        : PharmaceuticalPrescriptionRevokerTests<OracleFixture>
    {

        #region Constructors

        public OraclePharmaceuticalPrescriptionRevokerTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IObjectTranslator<IEvent, StoredEvent> CreateEventTranslator()
        {
            return new StoredEventTranslator(DataContractSerializerWrapper.Create(new UTF8Encoding(false)));
        }

        protected override ISession CreateSession()
        {
            var configuration = new BelgianOracleHealthcareConfiguration(OracleConnectionFactory.ConnectionString);
            return configuration.BuildSessionFactory().OpenSession();
        }

        #endregion Methods

    }
}
