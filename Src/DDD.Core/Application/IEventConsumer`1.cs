namespace DDD.Core.Application
{
    public interface IEventConsumer<out TContext> : IEventConsumer
        where TContext : class, IBoundedContext
    {
        #region Properties

        TContext Context { get; }

        #endregion Properties
    }
}
