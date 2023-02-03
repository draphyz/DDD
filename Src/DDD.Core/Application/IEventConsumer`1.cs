namespace DDD.Core.Application
{
    using Domain;

    public interface IEventConsumer<out TContext> : IEventConsumer
        where TContext : BoundedContext
    {
        #region Properties

        TContext Context { get; }

        #endregion Properties
    }
}
