using Xunit;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
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

    }
}
