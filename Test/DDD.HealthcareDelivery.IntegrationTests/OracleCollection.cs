using Xunit;

namespace DDD.HealthcareDelivery
{
    [CollectionDefinition("Oracle")]
    public class OracleCollection : ICollectionFixture<OracleFixture>
    {
    }
}
