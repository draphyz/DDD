using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerFailedEventStreamUpdaterTests : FailedEventStreamUpdaterTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerFailedEventStreamUpdaterTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}