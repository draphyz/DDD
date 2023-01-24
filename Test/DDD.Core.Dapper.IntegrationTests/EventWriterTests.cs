using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public abstract class EventWriterTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected EventWriterTests(TFixture fixture)
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
            this.Fixture.ExecuteScriptFromResources("WriteEvents");
            var handler = new EventWriter<TestContext>(this.ConnectionProvider);
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
            this.Fixture.ExecuteScriptFromResources("WriteEvents");
            var handler = new EventWriter<TestContext>(this.ConnectionProvider);
            var command = CreateCommand();
            // Act
            Func<Task> handle = async () => await handler.HandleAsync(command);
            // Assert
            await handle.Should().NotThrowAsync();
        }

        private static WriteEvents CreateCommand()
        {
            return new WriteEvents
            {
                Events = new []
                {
                    new Event
                    {
                        EventId = new Guid("f7df5bd0-8763-677e-7e6b-3a0044746810"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2021, 11, 18, 10, 1, 23, 528),
                        Body = "{\"boxId\":2,\"occurredOn\":\"2021-11-18T10:01:23.5277314+01:00\"}",
                        BodyFormat = "JSON",
                        StreamId = "2",
                        StreamType = "MessageBox",
                        IssuedBy = "Dr Maboul"
                    }
                }
            };
        }

        #endregion Methods

    }
}
