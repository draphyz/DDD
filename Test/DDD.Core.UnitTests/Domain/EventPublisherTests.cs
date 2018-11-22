using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace DDD.Core.Domain
{
    using Collections;

    [Trait("Category", "Unit")]
    public class EventPublisherTests
    {
        #region Methods

        public static IEnumerable<object[]> SubscribersToOtherEventsThanThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = new EventPublisher();
            publisher.Subscribe(fakeSubscriber1);
            publisher.Subscribe(fakeSubscriber2);
            publisher.Subscribe(fakeSubscriber3);
            publisher.Subscribe(fakeSubscriber4);
            yield return new object[]
            {
                publisher,
                new FakeEvent1(),
                new IEventHandler[] { fakeSubscriber2, fakeSubscriber3, fakeSubscriber4 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                new IEventHandler[] { fakeSubscriber3, fakeSubscriber4 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IEventHandler[] { fakeSubscriber1, fakeSubscriber2 }
            };
        }

        public static IEnumerable<object[]> SubscribersToThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = new EventPublisher();
            publisher.Subscribe(fakeSubscriber1);
            publisher.Subscribe(fakeSubscriber2);
            publisher.Subscribe(fakeSubscriber3);
            publisher.Subscribe(fakeSubscriber4);
            yield return new object[]
            {
                publisher,
                new FakeEvent1(),
                new IEventHandler[] { fakeSubscriber1 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                new IEventHandler[] { fakeSubscriber1, fakeSubscriber2 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IEventHandler[] { fakeSubscriber3, fakeSubscriber4 }
            };
        }

        public static IEnumerable<object[]> UnSubscribersToThisEvent()
        {
            var fakeSubscriber1 = FakeSubscriber<FakeEvent1>();
            var fakeSubscriber2 = FakeSubscriber<FakeEvent2>();
            var fakeSubscriber3 = FakeSubscriber<FakeEvent3>();
            var fakeSubscriber4 = FakeSubscriber<FakeEvent3>();
            var publisher = new EventPublisher();
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
                new IEventHandler[] { fakeSubscriber1 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                new IEventHandler[] { fakeSubscriber1, fakeSubscriber2 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IEventHandler[] { fakeSubscriber3, fakeSubscriber4 }
            };
        }

        [Theory]
        [MemberData(nameof(SubscribersToThisEvent))]
        public void Publish_WhenCalled_CallsSubscribersToThisEvent(EventPublisher publisher,
                                                                   IEvent @event,
                                                                   IEnumerable<IEventHandler> subscribersToThisEvent)
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
        public void Publish_WhenCalled_DoesNotCallUnSubscribersToThisEvent(EventPublisher publisher,
                                                                           IEvent @event,
                                                                           IEnumerable<IEventHandler> unSubscribersToThisEvent)
        {
            // Arrange
            unSubscribersToThisEvent.ForEach(s => s.ClearReceivedCalls());
            // Act
            publisher.Publish(@event);
            // Assert
            Assert.All(unSubscribersToThisEvent, s => s.DidNotReceive().Handle(Arg.Any<IEvent>()));
        }

        [Theory]
        [MemberData(nameof(SubscribersToOtherEventsThanThisEvent))]
        public void Publish_WhenCalled_DoesNotCallSubscribersToOtherEvents(EventPublisher publisher,
                                                                           IEvent @event,
                                                                           IEnumerable<IEventHandler> subscribersToOtherEvents)
        {
            // Arrange
            subscribersToOtherEvents.ForEach(s => s.ClearReceivedCalls());
            // Act
            publisher.Publish(@event);
            // Assert
            Assert.All(subscribersToOtherEvents, s => s.DidNotReceive().Handle(Arg.Any<IEvent>()));
        }

        private static IEventHandler<TEvent> FakeSubscriber<TEvent>() where TEvent : IEvent
        {
            var fakeSubscriber = Substitute.For<IEventHandler<TEvent>>();
            fakeSubscriber.EventType.Returns(typeof(TEvent));
            return fakeSubscriber;
        }

        #endregion Methods
    }
}