using Xunit;

namespace DDD.HealthcareDelivery.Application
{
    [CollectionDefinition("Oracle")]
    public class OracleCollection : ICollectionFixture<OracleFixture>
    {
    }
}
