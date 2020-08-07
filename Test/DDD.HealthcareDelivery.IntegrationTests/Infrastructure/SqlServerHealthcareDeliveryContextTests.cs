using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("SqlServer")]
    public class SqlServerHealthcareDeliveryContextTests : HealthcareDeliveryContextTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerHealthcareDeliveryContextTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}
