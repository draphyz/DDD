using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Defines a factory for creating DbContext instances.
    /// </summary>
    /// <remarks>Interface existing in versions 6.0 and 7.0 of Entity Framework Core.</remarks>
    public interface IDbContextFactory<TContext> where TContext : DbContext
    {
        #region Methods

        /// <summary>
        /// Creates a new DbContext instance.
        /// </summary>
        TContext CreateDbContext();

        /// <summary>
        /// Creates a new DbContext instance in an async context.
        /// </summary>
        Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default);

        #endregion Methods


    }
}
