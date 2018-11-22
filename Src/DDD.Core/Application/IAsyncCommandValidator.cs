namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a method that validates asynchronously a command of a specified type.
    /// </summary>
    public interface IAsyncCommandValidator<in TCommand> : IAsyncObjectValidator<TCommand>
        where TCommand : class, ICommand
    {
    }
}