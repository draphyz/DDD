using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerFailedEventStreamsFinderTests : FailedEventStreamsFinderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerFailedEventStreamsFinderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}