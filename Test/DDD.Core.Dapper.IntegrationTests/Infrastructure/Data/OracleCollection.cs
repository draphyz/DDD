using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [CollectionDefinition("Oracle")]
    public class OracleCollection : ICollectionFixture<OracleFixture>
    {
    }
}
