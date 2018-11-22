using Xunit;

namespace DDD.HealthcareDelivery
{
    [CollectionDefinition("SqlServer")]
    public class SqlServerCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
