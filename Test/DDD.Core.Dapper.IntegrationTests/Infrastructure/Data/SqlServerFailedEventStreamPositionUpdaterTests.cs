using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerFailedEventStreamPositionUpdaterTests : FailedEventStreamPositionUpdaterTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerFailedEventStreamPositionUpdaterTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}