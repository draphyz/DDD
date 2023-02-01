using NSubstitute;
using System.Collections.Generic;
using Xunit;
using SimpleInjector;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Collections;

    public class EventPublisherTests
    {

        #region Methods

        public static IEnumerable<object[]> HandlersOfOtherEventsThanThisEvent()
        {
            var fakeHandler1 = FakeHandler<FakeEvent1>();
            var fakeHandler2 = FakeHandler<FakeEvent2>();
            var fakeHandler3 = FakeHandler<FakeEvent3>();
            var fakeHandler4 = FakeHandler<FakeEvent3>();
            var container = new Container();
            container.Collection.Register(fakeHandler1);
            container.Collection.Register(fakeHandler2);
            container.Collection.Register(fakeHandler3, fakeHandler4);
            var publisher = new EventPublisher(container);
            yield return new object[]
            {
                publisher,
                new FakeEvent1(),
                new IAsyncEventHandler[] { fakeHandler2, fakeHandler3, fakeHandler4 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                new IAsyncEventHandler[] { fakeHandler3, fakeHandler4 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IAsyncEventHandler[] { fakeHandler1, fakeHandler2 }
            };
        }

        public static IEnumerable<object[]> HandlersOfThisEvent()
        {
            var fakeHandler1 = FakeHandler<FakeEvent1>();
            var fakeHandler2 = FakeHandler<FakeEvent2>();
            var fakeHandler3 = FakeHandler<FakeEvent3>();
            var fakeHandler4 = FakeHandler<FakeEvent3>();
            var container = new Container();
            container.Collection.Register(fakeHandler1);
            container.Collection.Register(fakeHandler2);
            container.Collection.Register(fakeHandler3, fakeHandler4);
            var publisher = new EventPublisher(container);
            yield return new object[]
            {
                publisher,
                new FakeEvent1(),
                new IAsyncEventHandler[] { fakeHandler1 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                new IAsyncEventHandler[] { fakeHandler1, fakeHandler2 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IAsyncEventHandler[] { fakeHandler3, fakeHandler4 }
            };
        }


        [Theory]
        [MemberData(nameof(HandlersOfThisEvent))]
        public async Task PublishAsync_WhenCalled_CallsHandlersOfThisEvent(EventPublisher publisher,
                                                                           IEvent @event,
                                                                           IEnumerable<IAsyncEventHandler> handlersOfThisEvent)
        {
            // Arrange
            handlersOfThisEvent.ForEach(s => s.ClearReceivedCalls());
            // Act
            await publisher.PublishAsync(@event);
            // Assert
            Assert.All(handlersOfThisEvent, s => s.Received(1).HandleAsync(@event));
        }


        [Theory]
        [MemberData(nameof(HandlersOfOtherEventsThanThisEvent))]
        public async Task PublishAsync_WhenCalled_DoesNotCallHandlersOfOtherEvents(EventPublisher publisher,
                                                                                   IEvent @event,
                                                                                   IEnumerable<IAsyncEventHandler> handlersOfOtherEvents)
        {
            // Arrange
            handlersOfOtherEvents.ForEach(s => s.ClearReceivedCalls());
            // Act
            await publisher.PublishAsync(@event);
            // Assert
            Assert.All(handlersOfOtherEvents, s => s.DidNotReceive().HandleAsync(Arg.Any<IEvent>()));
        }

        private static IAsyncEventHandler<TEvent> FakeHandler<TEvent>() where TEvent : class, IEvent
        {
            var fakeHandler = Substitute.For<IAsyncEventHandler<TEvent>>();
            fakeHandler.EventType.Returns(typeof(TEvent));
            return fakeHandler;
        }

        #endregion Methods

    }

}