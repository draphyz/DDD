namespace DDD.Core.Application
{
    using Core.Domain;

    /// <summary>
    /// Represents an action that should be taken.
    /// </summary>
    public interface ICommand : IMessage
    {
    }
}