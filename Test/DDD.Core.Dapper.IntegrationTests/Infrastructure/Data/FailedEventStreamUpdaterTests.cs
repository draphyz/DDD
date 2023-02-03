using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class FailedEventStreamUpdaterTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected FailedEventStreamUpdaterTests(TFixture fixture)
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
            Fixture.ExecuteScriptFromResources("UpdateFailedEventStream");
            var handler = new FailedEventStreamUpdater<TestContext>(ConnectionProvider);
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
            Fixture.ExecuteScriptFromResources("UpdateFailedEventStream");
            var handler = new FailedEventStreamUpdater<TestContext>(ConnectionProvider);
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

        private static UpdateFailedEventStream CreateCommand()
        {
            return new UpdateFailedEventStream
            {
                StreamId = "2",
                StreamType = "MessageBox",
                StreamSource = "COL",
                StreamPosition = new Guid("0a77707a-c147-9e1b-883a-08da0e368664"),
                EventId = new Guid("e10add4d-1851-7ede-883b-08da0e368664"),
                EventType = "DDD.Collaboration.Domain.Messages.MessageBoxDisabled, DDD.Collaboration.Messages",
                ExceptionTime = new DateTime(2021, 11, 20),
                ExceptionType = "System.NotImplementedException, mscorlib",
                ExceptionMessage = "The method or operation is not implemented.",
                ExceptionSource = "DDD.IdentityManagement, DDD.IdentityManagement.Application.MessageBoxDisabledEventHandler, Void Handle()",
                ExceptionInfo = "System.NotImplementedException: The method or operation is not implemented.",
                BaseExceptionType = "System.NotImplementedException, mscorlib",
                BaseExceptionMessage = "The method or operation is not implemented.",
                RetryCount = 1,
                RetryMax = 5,
                RetryDelays = new[]
                {
                    new IncrementalDelay{ Delay = 10 },
                    new IncrementalDelay{ Delay = 60 },
                    new IncrementalDelay{ Delay = 360 }
                }
            };
        }

        #endregion Methods
    }
}
