using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace DDD.Core.Domain
{
    using Collections;

    [Trait("Category", "Unit")]
    public class DomainEventPublisherTests
    {
        #region Methods

        public static IEnumerable<object[]> SubscribersToOtherEventsThanThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = new DomainEventPublisher();
            publisher.Subscribe(fakeSubscriber1);
            publisher.Subscribe(fakeSubscriber2);
            publisher.Subscribe(fakeSubscriber3);
            publisher.Subscribe(fakeSubscriber4);
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

        public static IEnumerable<object[]> SubscribersToThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = new DomainEventPublisher();
            publisher.Subscribe(fakeSubscriber1);
            publisher.Subscribe(fakeSubscriber2);
            publisher.Subscribe(fakeSubscriber3);
            publisher.Subscribe(fakeSubscriber4);
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

        public static IEnumerable<object[]> UnSubscribersToThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = new DomainEventPublisher();
            publisher.Subscribe(fakeSubscriber1);
            publisher.Subscribe(fakeSubscriber2);
            publisher.Subscribe(fakeSubscriber3);
            publisher.Subscribe(fakeSubscriber4);
            publisher.UnSubscribe(fakeSubscriber1);
            publisher.UnSubscribe(fakeSubscriber2);
            publisher.UnSubscribe(fakeSubscriber3);
            publisher.UnSubscribe(fakeSubscriber4);
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
        [MemberData(nameof(UnSubscribersToThisEvent))]
        public void Publish_WhenCalled_DoesNotCallUnSubscribersToThisEvent(DomainEventPublisher publisher,
                                                                           IDomainEvent @event,
                                                                           IEnumerable<IDomainEventHandler> unSubscribersToThisEvent)
        {
            // Arrange
            unSubscribersToThisEvent.ForEach(s => s.ClearReceivedCalls());
            // Act
            publisher.Publish(@event);
            // Assert
            Assert.All(unSubscribersToThisEvent, s => s.DidNotReceive().Handle(Arg.Any<IDomainEvent>()));
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

        private static IDomainEventHandler<TEvent> FakeSubscriber<TEvent>() where TEvent : IDomainEvent
        {
            var fakeSubscriber = Substitute.For<IDomainEventHandler<TEvent>>();
            fakeSubscriber.EventType.Returns(typeof(TEvent));
            return fakeSubscriber;
        }

        #endregion Methods
    }
}