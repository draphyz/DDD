using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    [Collection("Oracle")]
    public class OracleMedicationOrdersByIdFinderTests : MedicationOrdersByIdFinderTests<OracleFixture>
    {

        #region Constructors

        public OracleMedicationOrdersByIdFinderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}