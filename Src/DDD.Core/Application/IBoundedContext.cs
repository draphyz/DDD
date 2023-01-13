namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a bounded context of the application.
    /// </summary>
    public interface IBoundedContext
    {

        #region Properties

        string Name { get; }

        string Code { get; }

        #endregion Properties

    }
}
