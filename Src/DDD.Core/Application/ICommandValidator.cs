namespace DDD.Core.Application
{
    using Validation; 

    /// <summary>
    /// Defines a method that validates a command of a specified type.
    /// </summary>
    public interface ICommandValidator<in TCommand> : IObjectValidator<TCommand>
        where TCommand : class, ICommand
    {
    }
}