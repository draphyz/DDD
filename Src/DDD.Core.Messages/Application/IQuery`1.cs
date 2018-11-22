namespace DDD.Core.Application
{
    /// <summary>
    /// Represents a request for a specified type of information.
    /// </summary>
    /// <typeparam name="TResult">The type of requested information.</typeparam>
    /// <seealso cref="IQuery" />
    public interface IQuery<TResult> : IQuery
    {
    }
}