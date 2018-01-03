namespace DDD.Core.Application
{
    /// <summary>
    /// Represents an object that encapsulates all informations needed to perform a query that provides a result of a specified type.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="IQuery" />
    public interface IQuery<TResult> : IQuery
    {
    }
}