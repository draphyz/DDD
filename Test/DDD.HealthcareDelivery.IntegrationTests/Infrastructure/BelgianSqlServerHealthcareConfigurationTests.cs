using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("SqlServer")]
    public class BelgianSqlServerHealthcareConfigurationTests : HealthcareConfigurationTests<SqlServerFixture>
    {
        #region Constructors

        public BelgianSqlServerHealthcareConfigurationTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override HealthcareConfiguration CreateConfiguration()
        {
            return new BelgianSqlServerHealthcareConfiguration(SqlServerConnectionFactory.ConnectionString);
        }

        #endregion Methods
    }
}
