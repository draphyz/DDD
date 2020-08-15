using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("SqlServer")]
    public class BelgianSqlServerHealthcareDeliveryConfigurationTests : HealthcareDeliveryConfigurationTests<SqlServerFixture>
    {
        #region Constructors

        public BelgianSqlServerHealthcareDeliveryConfigurationTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override HealthcareDeliveryConfiguration CreateConfiguration()
        {
            return new BelgianSqlServerHealthcareDeliveryConfiguration(SqlServerConnectionFactory.ConnectionString);
        }

        #endregion Methods
    }
}
