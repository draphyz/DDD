using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerFailedEventStreamExcluderTests : FailedEventStreamExcluderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerFailedEventStreamExcluderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}