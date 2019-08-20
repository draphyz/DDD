using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("Oracle")]
    public class BelgianOracleHealthcareConfigurationTests : HealthcareConfigurationTests<OracleFixture>
    {
        #region Constructors

        public BelgianOracleHealthcareConfigurationTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override HealthcareConfiguration CreateConfiguration()
        {
            return new BelgianOracleHealthcareConfiguration(OracleConnectionFactory.ConnectionString);
        }

        #endregion Methods
    }
}