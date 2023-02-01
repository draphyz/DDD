using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [CollectionDefinition("SqlServer")]
    public class SqlServerCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
