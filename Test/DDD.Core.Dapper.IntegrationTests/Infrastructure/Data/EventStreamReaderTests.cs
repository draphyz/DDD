using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class EventStreamReaderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected EventStreamReaderTests(TFixture fixture)
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

        public static IEnumerable<object[]> QueriesAndResults_1()
        {
            yield return new object[]
            {
                new ReadEventStream
                {
                    StreamType = "MessageBox"
                },
                new []
                {
                    new Event
                    {
                        Body = "{\"boxId\":2,\"occurredOn\":\"2021-11-18T10:01:23.5277314+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2021, 11, 18, 10, 1, 23, 528),
                        StreamId = "2",
                        StreamType = "MessageBox",
                        IssuedBy = "Dr Maboul"
                    },
                    new Event
                    {
                        Body = "{\"boxId\":3,\"occurredOn\":\"2022-01-19T14:22:00.3683143+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 19, 14, 22, 0, 368),
                        StreamId = "3",
                        StreamType = "MessageBox",
                        IssuedBy = "Dr Folamour"
                    }
                }
            };
            yield return new object[]
            {
                new ReadEventStream
                {
                    StreamType = "Message",
                    ExcludedStreamIds = new [] { "1", "2", "5"}
                },
                new []
                {
                    new Event
                    {
                        Body = "{\"messageId\":3,\"occurredOn\":\"2021-11-16T00:00:00+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2021, 11, 16),
                        StreamId = "3",
                        StreamType = "Message",
                        IssuedBy = "Dr Maboul"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":3,\"occurredOn\":\"2021-11-18T10:01:30.7417655+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2021, 11, 18, 10, 1, 30, 742),
                        StreamId = "3",
                        StreamType = "Message",
                        IssuedBy = "Dr Maboul"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":4,\"occurredOn\":\"2022-01-20T09:24:59.6050066+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageCreated, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20, 9, 24, 59, 605),
                        StreamId = "4",
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":4,\"occurredOn\":\"2022-01-20T09:25:06.077499+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageSent, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20 , 9, 25, 6, 77),
                        StreamId = "4",
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    }
                }
            };
            yield return new object[]
            {
                new ReadEventStream
                {
                    StreamType = "Message",
                    Top = 3
                },
                new []
                {
                    new Event
                    {
                        Body = "{\"messageId\":1,\"occurredOn\":\"2021-11-05T00:00:00+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2021, 11, 05),
                        StreamId = "1",
                        StreamType = "Message",
                        IssuedBy = "Dr Maboul"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":1,\"occurredOn\":\"2021-11-18T10:01:30.4670777+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageReceived, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2021, 11, 18, 10, 1, 30, 467),
                        StreamId = "1",
                        StreamType = "Message",
                        IssuedBy = "Dr Maboul"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":2,\"occurredOn\":\"2021-11-09T00:00:00+01:00\"}",
                        BodyFormat = "JSON",
                        EventType = "DDD.Collaboration.Domain.Messages.MessageDelivered, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2021, 11, 09),
                        StreamId = "2",
                        StreamType = "Message",
                        IssuedBy = "Dr Maboul"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults_1))]
        public void Handle_WhenCalled_ReturnsExpectedResults_1(ReadEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("ReadEventStream");
            var handler = new EventStreamReader<TestContext>(ConnectionProvider);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering()
                                                                               .Excluding(e => e.EventId));
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults_1))]
        public async Task HandleAsync_WhenCalled_ReturnsExpectedResults_1(ReadEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("ReadEventStream");
            var handler = new EventStreamReader<TestContext>(ConnectionProvider);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering()
                                                                               .Excluding(e => e.EventId));
        }

        public void Dispose()
        {
            ConnectionProvider.Dispose();
        }

        #endregion Methods

    }
}
