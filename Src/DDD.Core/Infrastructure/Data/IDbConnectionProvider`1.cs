namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using DDD.Core.Domain;

    /// <summary>
    /// Provides and shares a database connection for a specific context between different components.
    /// </summary>
    public interface IDbConnectionProvider<out TContext> 
        : IDbConnectionProvider where TContext : BoundedContext
    {

        #region Properties

        TContext Context { get; }

        #endregion Properties

    }
}
