using System.Data.Entity;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Defines a method that creates a <see cref="DbContext"/> of a specified type. 
    /// </summary>
    public interface IAsyncDbContextFactory<TContext>
        where TContext : DbContext
    {

        #region Methods

        Task<TContext> CreateContextAsync();

        #endregion Methods

    }
}
