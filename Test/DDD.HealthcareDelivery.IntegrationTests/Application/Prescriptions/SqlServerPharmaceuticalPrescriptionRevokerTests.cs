using Xunit;
using System.Text;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Domain;
    using Core.Infrastructure.Serialization;
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

        protected override HealthcareContext CreateContext()
        {
            return new SqlServerHealthcareContext("Sqlserver");
        }

        protected override EventTranslator CreateEventTranslator()
        {
            return new EventTranslator(DataContractSerializerWrapper.Create(Encoding.Unicode));
        }

        #endregion Methods

    }
}
