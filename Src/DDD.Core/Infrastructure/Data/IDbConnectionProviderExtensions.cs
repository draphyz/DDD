using EnsureThat;
using System.Data.Common;
using DDD.Data;
using System.Threading.Tasks;
using System.Threading;

namespace DDD.Core.Infrastructure.Data
{
    using Threading;

    public static class IDbConnectionProviderExtensions
    {

        #region Methods

        /// <summary>
        /// Opens the shared connection if closed and returns the connection.
        /// </summary>
        public static DbConnection GetOpenConnection(this IDbConnectionProvider connectionProvider)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
            connectionProvider.Connection.OpenIfClosed();
            return connectionProvider.Connection;
        }

        /// <summary>
        /// Opens the shared connection if closed.
        /// </summary>
        public static void OpenConnectionIfClosed(this IDbConnectionProvider connectionProvider)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
            connectionProvider.Connection.OpenIfClosed();
        }

        /// <summary>
        /// Opens asynchronously the shared connection if closed.
        /// </summary>
        public static async Task OpenConnectionIfClosedAsync(this IDbConnectionProvider connectionProvider,
                                                             CancellationToken cancellationToken = default)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
            await new SynchronizationContextRemover();
            await connectionProvider.Connection.OpenIfClosedAsync(cancellationToken);
        }

        /// <summary>
        /// Opens asynchronously the shared connection if closed and returns the connection.
        /// </summary>
        public async static Task<DbConnection> GetOpenConnectionAsync(this IDbConnectionProvider connectionProvider,
                                                                      CancellationToken cancellationToken = default)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
            await new SynchronizationContextRemover();
            await connectionProvider.Connection.OpenIfClosedAsync(cancellationToken);
            return connectionProvider.Connection;
        }

        #endregion Methods

    }


}
