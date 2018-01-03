using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    [Collection("SqlServer")]
    public class SqlServerMedicationOrdersByIdFinderTests : MedicationOrdersByIdFinderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerMedicationOrdersByIdFinderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}