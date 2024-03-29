﻿using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class FailedEventStreamIncluderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected FailedEventStreamIncluderTests(TFixture fixture)
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
            Fixture.ExecuteScriptFromResources("IncludeFailedEventStream");
            var handler = new FailedEventStreamDeleter<TestContext>(ConnectionProvider);
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
            Fixture.ExecuteScriptFromResources("IncludeFailedEventStream");
            var handler = new FailedEventStreamDeleter<TestContext>(ConnectionProvider);
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

        private static IncludeFailedEventStream CreateCommand()
        {
            return new IncludeFailedEventStream
            {
                Id = "2",
                Type = "MessageBox",
                Source = "COL"
            };
        }

        #endregion Methods

    }
}
