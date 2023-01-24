using Conditions;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;

namespace DDD.Core.Infrastructure.Testing
{
    using Application;
    using DDD.Core.Domain;

    public static class IDbFixtureExtensions
    {

        #region Methods

        public static DbConnection CreateOpenConnection<TContext>(this IDbFixture<TContext> fixture, bool pooling = true)
            where TContext : BoundedContext
        {
            Condition.Requires(fixture, nameof(fixture)).IsNotNull();
            var connection = fixture.CreateConnection(pooling);
            connection.Open();
            return connection;
        }

        public static async Task<DbConnection> CreateOpenConnectionAsync<TContext>(this IDbFixture<TContext> fixture,
                                                                                   CancellationToken cancellationToken = default,
                                                                                   bool pooling = true)
            where TContext : BoundedContext
        {
            Condition.Requires(fixture, nameof(fixture)).IsNotNull();
            var connection = fixture.CreateConnection(pooling);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            return connection;
        }

        #endregion Methods

    }
}
