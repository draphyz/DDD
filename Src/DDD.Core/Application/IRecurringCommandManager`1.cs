namespace DDD.Core.Application
{
    /// <summary>
    /// Represents a component that registers, schedules and processes recurring commands in a specific bounded context.
    /// </summary>
    public interface IRecurringCommandManager<out TContext> : IRecurringCommandManager
        where TContext : class, IBoundedContext
    {
        #region Properties

        TContext Context { get; }

        #endregion Properties
    }
}
