using Dapper;
using FluentAssertions;
using Xunit;

namespace Xperthis.Core.Infrastructure.Data
{
    [Trait("Category", "Integration")]
    public class SqlServerFixtureTests
    {
        #region Methods

        [Fact]
        public void ExecuteScript_WhenCalled_AffectsDatabase()
        {
            // Arrange
            var fixture = new FakeSqlServerFixture();
            // Act
            fixture.ExecuteScript(SqlServerScripts.TestScript);
            // Assert
            using (var connection = fixture.ConnectionFactory.CreateConnection())
            {
                var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM [Test].[dbo].[PROV_PHYSIQUE]");
                count.Should().Be(1);
            }
        }

        #endregion Methods
    }
}