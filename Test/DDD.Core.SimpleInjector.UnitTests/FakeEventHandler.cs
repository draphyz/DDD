using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using DDD.Core.Domain;
    using System.Threading.Tasks;

    public class FakeEventHandler 
        : ISyncEventHandler<FakeEvent, FakeContext>, 
          IAsyncEventHandler<FakeEvent, FakeContext>
    {

        #region Properties

        public FakeContext Context { get; } = new FakeContext();

        public Type EventType => typeof(FakeEvent);

        BoundedContext ISyncEventHandler.Context => this.Context;

        BoundedContext IAsyncEventHandler.Context => this.Context;

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
