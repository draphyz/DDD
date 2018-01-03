using System.Data.Entity;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Defines a method that creates a <see cref="DbContext"/> of a specified type. 
    /// </summary>
    public interface IDbContextFactory<out TContext>
        where TContext : DbContext
    {

        #region Methods

        TContext CreateContext();

        #endregion Methods

    }
}
