using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    [Collection("SqlServer")]
    public class SqlServerPharmaceuticalPrescriptionsByPatientFinderTests : PharmaceuticalPrescriptionsByPatientFinderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerPharmaceuticalPrescriptionsByPatientFinderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}