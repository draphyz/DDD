using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DDD.Core.Domain
{
    using Collections;

    [Trait("Category", "Unit")]
    public class DomainEventPublisherTests
    {
        #region Methods

        public static IEnumerable<object[]> SubscribersToOtherEventsThanTheseEvents()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = PublisherWithSubscribers(fakeSubscriber1,
                                                     fakeSubscriber2,
                                                     fakeSubscriber3,
                                                     fakeSubscriber4);
            yield return new object[]
            {
                publisher,
                new IDomainEvent[] { new FakeEvent1(), new FakeEvent3() },
                new IDomainEventHandler[] { fakeSubscriber2 }
            };
            yield return new object[]
            {
                publisher,
                new IDomainEvent[] { new FakeEvent3() },
                new IDomainEventHandler[] { fakeSubscriber1, fakeSubscriber2 }
            };
        }

        public static IEnumerable<object[]> SubscribersToOtherEventsThanThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = PublisherWithSubscribers(fakeSubscriber1,
                                                     fakeSubscriber2,
                                                     fakeSubscriber3,
                                                     fakeSubscriber4);
            yield return new object[]
            {
                publisher,
                new FakeEvent1(),
                new IDomainEventHandler[] { fakeSubscriber2, fakeSubscriber3, fakeSubscriber4 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                new IDomainEventHandler[] { fakeSubscriber3, fakeSubscriber4 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IDomainEventHandler[] { fakeSubscriber1, fakeSubscriber2 }
            };
        }

        public static IEnumerable<object[]> SubscribersToTheseEvents()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = PublisherWithSubscribers(fakeSubscriber1,
                                                     fakeSubscriber2,
                                                     fakeSubscriber3,
                                                     fakeSubscriber4);
            yield return new object[]
            {
                publisher,
                new IDomainEvent[] { new FakeEvent1(), new FakeEvent3() },
                new IDomainEventHandler[] { fakeSubscriber1, fakeSubscriber3, fakeSubscriber4 }
            };
            yield return new object[]
            {
                publisher,
                new IDomainEvent[] { new FakeEvent2(), new FakeEvent3() },
                new IDomainEventHandler[] { fakeSubscriber1, fakeSubscriber2, fakeSubscriber3, fakeSubscriber4 }
            };
        }

        public static IEnumerable<object[]> SubscribersToThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = PublisherWithSubscribers(fakeSubscriber1,
                                                     fakeSubscriber2,
                                                     fakeSubscriber3,
                                                     fakeSubscriber4);
            yield return new object[]
            {
                publisher,
                new FakeEvent1(),
                new IDomainEventHandler[] { fakeSubscriber1 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                new IDomainEventHandler[] { fakeSubscriber1, fakeSubscriber2 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IDomainEventHandler[] { fakeSubscriber3, fakeSubscriber4 }
            };
        }

        [Fact]
        public void AllSubscribers_WhenCalled_ReturnsAllRegistredSubscribers()
        {
            // Arrange
            var subscribers = new IDomainEventHandler[]
            {
                FakeSubscriber<FakeEvent1>(),
                FakeSubscriber<FakeEvent2>(),
                FakeSubscriber<FakeEvent3>()
            };
            var publisher = PublisherWithSubscribers(subscribers);
            // Act
            var allSubscribers = publisher.AllSubscribers();
            // Assert
            allSubscribers.Should().BeEquivalentTo(subscribers);
        }

        [Fact]
        public void HasSubscribers_OnPublisherWithoutSubscribers_ReturnsFalse()
        {
            // Arrange
            var publisher = PublisherWithoutSubscribers();
            // Act
            var hasSubscribers = publisher.HasSubscribers();
            // Assert
            hasSubscribers.Should().BeFalse();
        }

        [Fact]
        public void HasSubscribers_OnPublisherWithSubscribers_ReturnsTrue()
        {
            // Arrange
            var publisher = PublisherWithSubscribers(FakeSubscriber<FakeEvent1>());
            // Act
            var hasSubscribers = publisher.HasSubscribers();
            // Assert
            hasSubscribers.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(SubscribersToThisEvent))]
        public void Publish_WhenCalled_CallsSubscribersToThisEvent(DomainEventPublisher publisher,
                                                                   IDomainEvent @event,
                                                                   IEnumerable<IDomainEventHandler> subscribersToThisEvent)
        {
            // Arrange
            subscribersToThisEvent.ForEach(s => s.ClearReceivedCalls());
            // Act
            publisher.Publish(@event);
            // Assert
            Assert.All(subscribersToThisEvent, s => s.Received(1).Handle(@event));
        }

        [Theory]
        [MemberData(nameof(SubscribersToOtherEventsThanThisEvent))]
        public void Publish_WhenCalled_DoesNotCallSubscribersToOtherEvents(DomainEventPublisher publisher,
                                                                           IDomainEvent @event,
                                                                           IEnumerable<IDomainEventHandler> subscribersToOtherEvents)
        {
            // Arrange
            subscribersToOtherEvents.ForEach(s => s.ClearReceivedCalls());
            // Act
            publisher.Publish(@event);
            // Assert
            Assert.All(subscribersToOtherEvents, s => s.DidNotReceive().Handle(Arg.Any<IDomainEvent>()));
        }

        [Theory]
        [MemberData(nameof(SubscribersToTheseEvents))]
        public void PublishAll_WhenCalled_CallsSubscribersToTheseEvents(DomainEventPublisher publisher,
                                                                        IEnumerable<IDomainEvent> events,
                                                                        IEnumerable<IDomainEventHandler> subscribersToTheseEvents)
        {
            // Arrange
            subscribersToTheseEvents.ForEach(s => s.ClearReceivedCalls());
            // Act
            publisher.PublishAll(events);
            // Assert
            Assert.All(subscribersToTheseEvents, s => s.Received().Handle(Arg.Is<IDomainEvent>(e => events.Contains(e))));
        }

        [Theory]
        [MemberData(nameof(SubscribersToOtherEventsThanTheseEvents))]
        public void PublishAll_WhenCalled_DoesNotCallSubscribersToOtherEvents(DomainEventPublisher publisher,
                                                                              IEnumerable<IDomainEvent> events,
                                                                              IEnumerable<IDomainEventHandler> subscribersToOtherEvents)
        {
            // Arrange
            subscribersToOtherEvents.ForEach(s => s.ClearReceivedCalls());
            // Act
            publisher.PublishAll(events);
            // Assert
            Assert.All(subscribersToOtherEvents, s => s.DidNotReceive().Handle(Arg.Any<IDomainEvent>()));
        }

        [Fact]
        public void Register_WhenCalled_AddsSubscriber()
        {
            // Arrange
            var publisher = PublisherWithoutSubscribers();
            var subscriber = Substitute.For<IDomainEventHandler<FakeEvent1>>();
            // Act
            publisher.Register(subscriber);
            // Assert
            publisher.AllSubscribers().Should().Contain(subscriber);
        }

        [Fact]
        public void RegisterAll_WhenCalled_AddsAllSubscribers()
        {
            // Arrange
            var publisher = PublisherWithoutSubscribers();
            var subscribers = new IDomainEventHandler[]
            {
                Substitute.For<IDomainEventHandler<FakeEvent1>>(),
                Substitute.For<IDomainEventHandler<FakeEvent2>>()
            };
            // Act
            publisher.RegisterAll(subscribers);
            // Assert
            publisher.AllSubscribers().Should().BeEquivalentTo(subscribers);
        }

        [Fact]
        public void Unregister_WhenCalled_DoesNotRemoveOtherSubscribers()
        {
            // Arrange
            var subscriber1 = FakeSubscriber<FakeEvent1>();
            var subscriber2 = FakeSubscriber<FakeEvent2>();
            var publisher = PublisherWithSubscribers(subscriber1, subscriber2);
            // Act
            publisher.Unregister(subscriber1);
            // Assert
            publisher.AllSubscribers().Should().Contain(subscriber2);
        }

        [Fact]
        public void Unregister_WhenCalled_RemovesSpecifiedSubscriber()
        {
            // Arrange
            var subscriber1 = FakeSubscriber<FakeEvent1>();
            var subscriber2 = FakeSubscriber<FakeEvent2>();
            var publisher = PublisherWithSubscribers(subscriber1, subscriber2);
            // Act
            publisher.Unregister(subscriber1);
            // Assert
            publisher.AllSubscribers().Should().NotContain(subscriber1);
        }

        [Fact]
        public void UnregisterAll_WhenCalled_RemovesAllSubscribers()
        {
            // Arrange
            var publisher = PublisherWithSubscribers(Substitute.For<IDomainEventHandler<FakeEvent1>>(),
                                                     Substitute.For<IDomainEventHandler<FakeEvent2>>());
            // Act
            publisher.UnregisterAll();
            // Assert
            publisher.AllSubscribers().Should().BeEmpty();
        }

        private static IDomainEventHandler<TEvent> FakeSubscriber<TEvent>() where TEvent : IDomainEvent
        {
            var fakeSubscriber = Substitute.For<IDomainEventHandler<TEvent>>();
            fakeSubscriber.EventType.Returns(typeof(TEvent));
            return fakeSubscriber;
        }

        private static DomainEventPublisher PublisherWithoutSubscribers() => new DomainEventPublisher();

        private static DomainEventPublisher PublisherWithSubscribers(params IDomainEventHandler[] subscribers)
        {
            return new DomainEventPublisher(subscribers);
        }

        #endregion Methods
    }
}