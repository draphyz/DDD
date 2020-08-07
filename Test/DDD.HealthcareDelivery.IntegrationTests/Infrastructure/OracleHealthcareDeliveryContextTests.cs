using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("Oracle")]
    public class OracleHealthcareDeliveryContextTests : HealthcareDeliveryContextTests<OracleFixture>
    {

        #region Constructors

        public OracleHealthcareDeliveryContextTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}