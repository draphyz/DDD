namespace DDD.Core.Application
{
    /// <summary>
    /// Defines methods that handle synchronously and asynchronously a command of a specified type.
    /// </summary>
    public interface ICommandHandler<in TCommand> : ISyncCommandHandler<TCommand>, IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {
    }
}