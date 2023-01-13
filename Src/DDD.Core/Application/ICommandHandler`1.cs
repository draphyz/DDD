namespace DDD.Core.Application
{
    /// <summary>
    /// Defines methods that handle synchronously and asynchronously a command of a specified type in a specific bounded context.
    /// </summary>
    public interface ICommandHandler<in TCommand, out TContext> : ISyncCommandHandler<TCommand, TContext>, IAsyncCommandHandler<TCommand, TContext>
        where TCommand : class, ICommand
        where TContext : class, IBoundedContext
    {
    }
}