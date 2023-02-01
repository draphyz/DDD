using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerFailedEventStreamIncluderTests : FailedEventStreamIncluderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerFailedEventStreamIncluderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}