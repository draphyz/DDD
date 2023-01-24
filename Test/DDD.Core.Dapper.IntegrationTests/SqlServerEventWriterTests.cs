using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerEventWriterTests : EventWriterTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerEventWriterTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}