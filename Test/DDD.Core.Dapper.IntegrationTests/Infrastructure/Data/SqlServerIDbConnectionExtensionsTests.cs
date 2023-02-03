using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerIDbConnectionExtensionsTests : IDbConnectionExtensionsTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerIDbConnectionExtensionsTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}