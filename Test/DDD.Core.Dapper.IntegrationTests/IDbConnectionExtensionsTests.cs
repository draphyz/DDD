using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    public abstract class IDbConnectionExtensionsTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected IDbConnectionExtensionsTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.ConnectionProvider = fixture.CreateConnectionProvider();
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<TestContext> ConnectionProvider { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
        }

        [Fact]
        public void NextId_WhenExistingRows_ReturnsExpectedId()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("NextId_ExistingRows");
            var connection = this.ConnectionProvider.GetOpenConnection();
            // Act
            var id = connection.NextId("TableWithId", "Id");
            // Assert
            id.Should().Be(5);
        }

        [Fact]
        public void NextId_WhenNoRow_ReturnsExpectedId()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("NextId_NoRow");
            var connection = this.ConnectionProvider.GetOpenConnection();
            // Act
            var id = connection.NextId("TableWithId", "Id");
            // Assert
            id.Should().Be(1);
        }

        [Fact]
        public async Task NextIdAsync_WhenExistingRows_ReturnsExpectedId()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("NextId_ExistingRows");
            var connection = await this.ConnectionProvider.GetOpenConnectionAsync();
            // Act
            var id = await connection.NextIdAsync("TableWithId", "Id");
            // Assert
            id.Should().Be(5);
        }

        [Fact]
        public async Task NextIdAsync_WhenNoRow_ReturnsExpectedId()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("NextId_NoRow");
            var connection = await this.ConnectionProvider.GetOpenConnectionAsync();
            // Act
            var id = await connection.NextIdAsync("TableWithId", "Id");
            // Assert
            id.Should().Be(1);
        }

        #endregion Methods

    }
}
