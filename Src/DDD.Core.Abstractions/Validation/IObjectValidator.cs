namespace DDD.Validation
{
    /// <summary>
    /// Defines methods that validate synchronously and asynchronously an object of a specified type.
    /// </summary>
    public interface IObjectValidator<in T> : ISyncObjectValidator<T>, IAsyncObjectValidator<T>
        where T : class
    {
    }
}