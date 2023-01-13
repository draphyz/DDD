namespace DDD.Core.Application
{
    /// <summary>
    /// Defines methods that validate synchronously and asynchronously  a command of a specified type.
    /// </summary>
    public interface ICommandValidator<in TCommand> : ISyncCommandValidator<TCommand>, IAsyncCommandValidator<TCommand>
        where TCommand : class, ICommand
    {
    }
}