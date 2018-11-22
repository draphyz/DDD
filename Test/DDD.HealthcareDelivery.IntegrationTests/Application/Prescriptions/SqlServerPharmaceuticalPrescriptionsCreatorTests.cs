using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Domain.Prescriptions;
    using Infrastructure.Prescriptions;
    using Infrastructure;

    [Collection("SqlServer")]
    public class SqlServerPharmaceuticalPrescriptionsCreatorTests
        : PharmaceuticalPrescriptionsCreatorTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerPharmaceuticalPrescriptionsCreatorTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IAsyncRepository<PharmaceuticalPrescription> CreateRepository()
        {
            return new PharmaceuticalPrescriptionRepository
            (
                new Domain.Prescriptions.BelgianPharmaceuticalPrescriptionTranslator(),
                new EventTranslator(DataContractSerializerWrapper.Create(Encoding.Unicode)),
                new SqlServerHealthcareContextFactory(this.Fixture.ConnectionFactory)
            );
        }

        #endregion Methods

    }
}
