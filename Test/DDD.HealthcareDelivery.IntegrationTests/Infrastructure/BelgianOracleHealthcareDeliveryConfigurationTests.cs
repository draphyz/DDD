using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("Oracle")]
    public class BelgianOracleHealthcareDeliveryConfigurationTests : HealthcareDeliveryConfigurationTests<OracleFixture>
    {
        #region Constructors

        public BelgianOracleHealthcareDeliveryConfigurationTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override HealthcareDeliveryConfiguration CreateConfiguration()
        {
            return new BelgianOracleHealthcareDeliveryConfiguration(OracleConnectionFactory.ConnectionString);
        }

        #endregion Methods
    }
}