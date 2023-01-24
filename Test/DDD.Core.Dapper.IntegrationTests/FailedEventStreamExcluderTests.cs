using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public abstract class FailedEventStreamExcluderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected FailedEventStreamExcluderTests(TFixture fixture)
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

        [Fact]
        public void Handle_WhenCalled_DoesNotThrowException()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("ExcludeFailedEventStream");
            var handler = new FailedEventStreamCreator<TestContext>(this.ConnectionProvider);
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
            this.Fixture.ExecuteScriptFromResources("ExcludeFailedEventStream");
            var handler = new FailedEventStreamCreator<TestContext>(this.ConnectionProvider);
            var command = CreateCommand();
            // Act
            Func<Task> handle = async () => await handler.HandleAsync(command);
            // Assert
            await handle.Should().NotThrowAsync();
        }

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
        }

        private static ExcludeFailedEventStream CreateCommand()
        {
            return new ExcludeFailedEventStream
            {
                StreamId = "2",
                StreamType = "MessageBox",
                StreamSource = "COL",                                                                    
                StreamPosition = new Guid("0a77707a-c147-9e1b-883a-08da0e368663"),
                EventId = new Guid("e10add4d-1851-7ede-883b-08da0e368663"),
                EventType = "DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages",
                ExceptionTime = new DateTime(2021, 11, 19),
                ExceptionType = "System.Exception, mscorlib",
                ExceptionMessage = "Invalid event",
                ExceptionSource = "DDD.IdentityManagement, DDD.IdentityManagement.Application.MessageBoxCreatedEventHandler, Void Handle()",
                ExceptionInfo = "System.Exception: Invalid event ---> System.Exception: Format not supported.\r\n   --- End of inner exception stack trace ---",
                BaseExceptionType = "System.Exception, mscorlib",
                BaseExceptionMessage = "Format not supported.",
                RetryCount = 0,
                RetryMax = 5,
                RetryDelays = new []
                {
                    new IncrementalDelay{ Delay = 10 },
                    new IncrementalDelay{ Delay = 60 },
                    new IncrementalDelay{ Delay = 360 }
                },
                BlockSize = 100
            };
        }

        #endregion Methods

    }
}
