using Xunit;

namespace DDD.HealthcareDelivery.Application
{
    [CollectionDefinition("SqlServer")]
    public class SqlServerCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
