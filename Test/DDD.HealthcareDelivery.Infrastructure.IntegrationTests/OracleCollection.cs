using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [CollectionDefinition("Oracle")]
    public class OracleCollection : ICollectionFixture<OracleFixture>
    {
    }
}
