using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("Oracle")]
    public class OracleHealthcareDeliveryContextTests : DbHealthcareDeliveryContextTests<OracleFixture>
    {

        #region Constructors

        public OracleHealthcareDeliveryContextTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}