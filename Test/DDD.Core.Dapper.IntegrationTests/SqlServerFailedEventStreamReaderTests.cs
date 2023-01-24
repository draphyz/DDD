using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    [Collection("SqlServer")]
    public class SqlServerFailedEventStreamReaderTests : EventsByStreamIdFinderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerFailedEventStreamReaderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<object[]> QueriesAndResults()
        {
            yield return new object[]
            {
                new ReadFailedEventStream
                {
                    StreamType = "Message",
                    StreamId = "5",
                    EventIdMin = new Guid("321ea720-affd-6c91-3782-3a0189c0f051"),
                    EventIdMax = new Guid("0096f748-41f4-2e2b-87f3-3a0189c1505f")
                },
                new []
                {
                    new Event
                    {
                        Body = "{\"messageId\":5,\"occurredOn\":\"2022-01-20T14:01:41.3251974+01:00\"}",
                        BodyFormat = "JSON",
                        EventId = new Guid("321ea720-affd-6c91-3782-3a0189c0f051"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageRead, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20, 14, 1, 41, 325),
                        StreamId = "5",  
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":5,\"source\":\"Inbox\",\"destination\":\"Binbox\",\"occurredOn\":\"2022-01-20T14:01:48.6157105+01:00\"}",
                        BodyFormat = "JSON",
                        EventId = new Guid("a224a074-c1d9-6c6f-0adc-3a0189c10ccc"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageSentToBin, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20, 14, 1, 48, 616),
                        StreamId = "5",
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":5,\"occurredOn\":\"2022-01-20T14:02:05.9149942+01:00\"}",
                        BodyFormat = "JSON",
                        EventId = new Guid("0096f748-41f4-2e2b-87f3-3a0189c1505f"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageDeleted, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20, 14, 2, 5, 915),
                        StreamId = "5",
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults))]
        public void Handle_WhenCalled_ReturnsExpectedResults(ReadFailedEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("ReadFailedEventStream");
            var handler = new FailedEventStreamReader<TestContext>(this.ConnectionProvider);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults))]
        public async Task HandleAsync_WhenCalled_ReturnsExpectedResults(ReadFailedEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("ReadFailedEventStream");
            var handler = new FailedEventStreamReader<TestContext>(this.ConnectionProvider);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        #endregion Methods

    }
}