namespace DDD.Core.Infrastructure.Data
{
    using Application;

    /// <summary>
    /// Provides and shares a database connection for a specific context between different components.
    /// </summary>
    /// <remarks>Usefull when you load assemblies of different contexts into a single application.</remarks>
    public interface IDbConnectionProvider<out TContext> 
        : IDbConnectionProvider where TContext : class, IBoundedContext
    {

        #region Properties

        TContext Context { get; }

        #endregion Properties

    }
}
