using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [CollectionDefinition("SqlServer")]
    public class SqlServerCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
