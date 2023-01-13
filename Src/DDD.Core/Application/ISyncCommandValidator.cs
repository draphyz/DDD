namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a method that validates synchronously a command of a specified type.
    /// </summary>
    public interface ISyncCommandValidator<in TCommand> : ISyncObjectValidator<TCommand>
        where TCommand : class, ICommand
    {
    }
}