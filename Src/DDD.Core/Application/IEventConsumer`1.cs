using DDD.Core.Domain;

namespace DDD.Core.Application
{
    public interface IEventConsumer<out TContext> : IEventConsumer
        where TContext : BoundedContext
    {
        #region Properties

        TContext Context { get; }

        #endregion Properties
    }
}
