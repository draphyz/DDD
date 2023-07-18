using EnsureThat;
using System.Data.Common;

using System;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    public class LazyDbConnectionProvider<TContext>
        : IDbConnectionProvider<TContext> where TContext : BoundedContext
    {

        #region Fields

        private readonly DbConnectionSettings<TContext> settings;
        private readonly Lazy<DbConnection> lazyConnection;
        private bool isDisposed;

        #endregion Fields

        #region Constructors

        public LazyDbConnectionProvider(TContext context, DbConnectionSettings<TContext> settings)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            this.Context = context;
            this.settings = settings;
            this.lazyConnection = new Lazy<DbConnection>(() => this.CreateConnection());
        }

        #endregion Constructors

        #region Properties

        public DbConnection Connection => this.lazyConnection.Value;

        public TContext Context { get; }

        BoundedContext IDbConnectionProvider.Context => this.Context;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                if (isDisposing)
                {
                    if (lazyConnection.IsValueCreated)
                        lazyConnection.Value.Dispose();
                }
                isDisposed = true;
            }
        }

        private DbConnection CreateConnection()
        {
            var providerFactory = DbProviderFactories.GetFactory(this.settings.ProviderName);
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = this.settings.ConnectionString;
            return connection;
        }

        #endregion Methods

    }
}
