using System.Data.Common;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Implements a method that creates a <see cref="DbConnection"/> based on a specified provider name and a specified connection string. 
    /// </summary>
    public abstract class DbConnectionFactory : IDbConnectionFactory
    {

        #region Fields

        private readonly string connectionString;
        private readonly DbProviderFactory providerFactory;

        #endregion Fields

        #region Constructors

        protected DbConnectionFactory(string providerName, string connectionString)
        {
            Condition.Requires(providerName, nameof(providerName)).IsNotNullOrWhiteSpace();
            Condition.Requires(connectionString, nameof(connectionString)).IsNotNullOrWhiteSpace();
            this.providerFactory = DbProviderFactories.GetFactory(providerName);
            this.connectionString = connectionString;
        }

        #endregion Constructors

        #region Methods

        public DbConnection CreateConnection()
        {
            var connection = this.providerFactory.CreateConnection();
            connection.ConnectionString = this.connectionString;
            return connection;
        }

        #endregion Methods

    }
}
