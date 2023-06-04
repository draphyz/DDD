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

        public static IEnumerable<object[]> HandlersOfOtherEventsOrContexts()
        {
            var fakeHandler1 = FakeHandler<FakeEvent1, FakeContext>();
            var fakeHandler2 = FakeHandler<FakeEvent2, FakeContext>();
            var fakeHandler3 = FakeHandler<FakeEvent3, FakeContext>();
            var fakeHandler4 = FakeHandler<FakeEvent1, FakeSourceContext>();
            var container = new Container();
            container.RegisterInstance(fakeHandler1);
            container.RegisterInstance(fakeHandler2);
            container.RegisterInstance(fakeHandler3);
            var publisher = new EventPublisher<FakeContext>(new FakeContext(), container);
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
                new IAsyncEventHandler[] { fakeHandler1, fakeHandler3, fakeHandler4 }
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                new IAsyncEventHandler[] { fakeHandler1, fakeHandler2, fakeHandler4 }
            };
        }

        public static IEnumerable<object[]> HandlerOfThisEventAndThisContext()
        {
            var fakeHandler1 = FakeHandler<FakeEvent1, FakeContext>();
            var fakeHandler2 = FakeHandler<FakeEvent2, FakeContext>();
            var fakeHandler3 = FakeHandler<FakeEvent3, FakeContext>();
            var container = new Container();
            container.RegisterInstance(fakeHandler1);
            container.RegisterInstance(fakeHandler2);
            container.RegisterInstance(fakeHandler3);
            var publisher = new EventPublisher<FakeContext>(new FakeContext(), container);
            yield return new object[]
            {
                publisher,
                new FakeEvent1(),
                fakeHandler1
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent2(),
                fakeHandler2
            };
            yield return new object[]
            {
                publisher,
                new FakeEvent3(),
                fakeHandler3
            };
        }


        [Theory]
        [MemberData(nameof(HandlerOfThisEventAndThisContext))]
        public async Task PublishAsync_WhenCalled_CallsHandlerOfThisEventAndThisContext(EventPublisher<FakeContext> publisher,
                                                                                        IEvent @event,
                                                                                        IAsyncEventHandler handlerOfThisEvent)
        {
            // Arrange
            var context = new MessageContext();
            handlerOfThisEvent.ClearReceivedCalls();
            // Act
            await publisher.PublishAsync(@event, context);
            // Assert
            await handlerOfThisEvent.Received(1).HandleAsync(@event, context);
        }


        [Theory]
        [MemberData(nameof(HandlersOfOtherEventsOrContexts))]
        public async Task PublishAsync_WhenCalled_DoesNotCallHandlersOfOtherEventsOrContexts(EventPublisher<FakeContext> publisher,
                                                                                             IEvent @event,
                                                                                             IEnumerable<IAsyncEventHandler> handlersOfOtherEvents)
        {
            // Arrange
            var context = new MessageContext();
            handlersOfOtherEvents.ForEach(s => s.ClearReceivedCalls());
            // Act
            await publisher.PublishAsync(@event, context);
            // Assert
            Assert.All(handlersOfOtherEvents, s => s.DidNotReceive().HandleAsync(Arg.Any<IEvent>(), context));
        }

        private static IAsyncEventHandler<TEvent, TContext> FakeHandler<TEvent, TContext>()
            where TEvent : class, IEvent
            where TContext : BoundedContext, new()
        {
            var fakeHandler = Substitute.For<IAsyncEventHandler<TEvent, TContext>>();
            fakeHandler.EventType.Returns(typeof(TEvent));
            fakeHandler.Context.Returns(new TContext());
            return fakeHandler;
        }

        #endregion Methods

    }

}