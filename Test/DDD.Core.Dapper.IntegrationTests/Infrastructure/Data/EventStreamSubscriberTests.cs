using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class EventStreamSubscriberTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected EventStreamSubscriberTests(TFixture fixture)
        {
            Fixture = fixture;
            ConnectionProvider = fixture.CreateConnectionProvider();
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<TestContext> ConnectionProvider { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public void Handle_WhenCalled_DoesNotThrowException()
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("SubscribeToEventStream");
            var handler = new EventStreamSubscriber<TestContext>(ConnectionProvider);
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
            Fixture.ExecuteScriptFromResources("SubscribeToEventStream");
            var handler = new EventStreamSubscriber<TestContext>(ConnectionProvider);
            var command = CreateCommand();
            // Act
            Func<Task> handle = async () => await handler.HandleAsync(command);
            // Assert
            await handle.Should().NotThrowAsync();
        }

        public void Dispose()
        {
            ConnectionProvider.Dispose();
        }

        private static SubscribeToEventStream CreateCommand()
        {
            return new SubscribeToEventStream
            {
                Type = "Message",
                Source = "COL",
                Position = Guid.Empty,
                RetryMax = 3,
                RetryDelays = new[] { new IncrementalDelay { Delay = 60, Increment = 30 } },
                BlockSize = 100
            };
        }

        #endregion Methods
    }
}
