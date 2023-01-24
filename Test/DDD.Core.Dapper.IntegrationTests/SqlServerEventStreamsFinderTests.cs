using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerEventStreamsFinderTests : EventStreamsFinderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerEventStreamsFinderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}