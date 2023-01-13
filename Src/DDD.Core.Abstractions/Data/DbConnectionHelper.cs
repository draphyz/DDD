using Conditions;
using System.Data.Common;

namespace DDD.Data
{
    /// <summary>
    /// Helper class for DateTime.
    /// </summary>
    public class DbConnectionHelper
    {

        #region Methods

        public static DbConnection CreateConnection(string providerName, string connectionString)
        {
            Condition.Requires(providerName, nameof(providerName)).IsNotNullOrWhiteSpace();
            Condition.Requires(connectionString, nameof(connectionString)).IsNotNullOrWhiteSpace();
            var providerFactory = DbProviderFactories.GetFactory(providerName);
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        #endregion Methods

    }
}
