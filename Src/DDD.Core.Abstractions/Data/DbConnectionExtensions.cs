using EnsureThat;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Data
{
    using Threading;

    public static class DbConnectionExtensions
    {

        #region Methods

        public static void OpenIfClosed(this DbConnection connection)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public static async Task OpenIfClosedAsync(this DbConnection connection, CancellationToken cancellationToken = default)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            await new SynchronizationContextRemover();
            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync(cancellationToken);
        }

        #endregion Methods

    }
}
