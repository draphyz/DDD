namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// Defines a method that validates asynchronously a command of a specified type.
    /// </summary>
    public interface ICommandValidatorAsync<in TCommand> : IObjectValidatorAsync<TCommand>
        where TCommand : class, ICommand
    {
    }
}