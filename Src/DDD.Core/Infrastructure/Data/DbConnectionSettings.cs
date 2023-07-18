using EnsureThat;
using System;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;
    
    public class DbConnectionSettings<TContext>
        where TContext : BoundedContext
    {

        #region Constructors

        public DbConnectionSettings(string providerName, string connectionString) 
        {
            Ensure.That(providerName, nameof(providerName)).IsNotNullOrWhiteSpace();
            Ensure.That(connectionString, nameof(connectionString)).IsNotNullOrWhiteSpace();
            this.ContextType = typeof(TContext);
            this.ProviderName = providerName;
            this.ConnectionString = connectionString;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the type of the associated context.
        /// </summary>
        public Type ContextType { get; }

        /// <summary>
        /// Gets the provider name.
        /// </summary>
        public string ProviderName { get; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectionString { get; }

        #endregion Properties

    }
}
