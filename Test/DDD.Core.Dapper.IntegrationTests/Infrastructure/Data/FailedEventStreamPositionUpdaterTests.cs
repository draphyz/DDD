using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class FailedEventStreamPositionUpdaterTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected FailedEventStreamPositionUpdaterTests(TFixture fixture)
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
        public void Handle_WhenCalled_DoesNotThrowException()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("UpdateFailedEventStreamPosition");
            var handler = new FailedEventStreamPositionUpdater<TestContext>(this.ConnectionProvider);
            var command = CreateCommand();
            // Act
            Action handle = () => handler.Handle(command);
            // Assert
            handle.Should().NotThrow();
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_DoesNotThrowException()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("UpdateFailedEventStreamPosition");
            var handler = new FailedEventStreamPositionUpdater<TestContext>(this.ConnectionProvider);
            var command = CreateCommand();
            // Act
            Func<Task> handle = async () => await handler.HandleAsync(command);
            // Assert
            await handle.Should().NotThrowAsync();
        }

        private static UpdateFailedEventStreamPosition CreateCommand()
        {
            return new UpdateFailedEventStreamPosition
            {
                Id = "2",
                Type = "MessageBox",
                Source = "COL",
                Position = new Guid("f7df5bd0-8763-677e-7e6b-3a0044746810")
            };
        }

        #endregion Methods

    }
}
