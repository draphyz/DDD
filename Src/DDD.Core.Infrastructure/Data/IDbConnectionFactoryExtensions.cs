using System.Data.Common;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    public static class IDbConnectionFactoryExtensions
    {

        #region Methods

        public static DbConnection CreateOpenConnection(this IDbConnectionFactory factory)
        {
            Condition.Requires(factory, nameof(factory)).IsNotNull();
            var connection = factory.CreateConnection();
            connection.Open();
            return connection;
        }

        #endregion Methods

    }
}
