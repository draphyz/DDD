using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    
    public class FakeEventHandler
        : ISyncEventHandler<FakeEvent, FakeContext>,
          IAsyncEventHandler<FakeEvent, FakeContext>
    {

        #region Properties

        public FakeContext Context { get; } = new FakeContext();

        public Type EventType => typeof(FakeEvent);

        BoundedContext ISyncEventHandler.Context => Context;

        BoundedContext IAsyncEventHandler.Context => Context;

        #endregion Properties

        #region Methods

        public void Handle(FakeEvent @event, IMessageContext context)
        {
        }

        public void Handle(IEvent @event, IMessageContext context)
        {
        }

        public Task HandleAsync(FakeEvent @event, IMessageContext context)
        {
            return Task.CompletedTask;
        }

        public Task HandleAsync(IEvent @event, IMessageContext context)
        {
            return Task.CompletedTask;
        }

        #endregion Methods

    }
}
