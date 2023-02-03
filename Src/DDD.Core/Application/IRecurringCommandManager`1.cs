namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Represents a component that registers, schedules and processes recurring commands in a specific bounded context.
    /// </summary>
    public interface IRecurringCommandManager<out TContext> : IRecurringCommandManager
        where TContext : BoundedContext
    {
        #region Properties

        TContext Context { get; }

        #endregion Properties
    }
}
