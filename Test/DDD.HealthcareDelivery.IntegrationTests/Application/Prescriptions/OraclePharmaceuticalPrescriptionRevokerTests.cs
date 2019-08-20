using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Domain.Prescriptions;
    using Infrastructure;
    using Core.Infrastructure.Data;

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

        protected override IAsyncRepository<PharmaceuticalPrescription, PrescriptionIdentifier> CreateRepository()
        {
            var configuration = new BelgianOracleHealthcareConfiguration(OracleConnectionFactory.ConnectionString);
            var session = configuration.BuildSessionFactory().OpenSession();
            return new NHibernateRepository<PharmaceuticalPrescription, PrescriptionIdentifier>
             (
                session,
                new StoredEventTranslator(DataContractSerializerWrapper.Create(new UTF8Encoding(false)))
            );
        }

        #endregion Methods

    }
}
