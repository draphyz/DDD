using Conditions;
using System;
using System.Data;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Adds extension method to the <see cref="IDbConnection"/>  interface.
    /// </summary>
    public static class IDbConnectionExtensions
    {

        #region Methods

        public static IDbStandardExpressions Expressions(this IDbConnection connection)
        {
            Condition.Requires(connection, nameof(connection)).IsNotNull();
            switch (connection.GetType().ToString())
            {
                case "System.Data.SqlClient.SqlConnection":
                case "Microsoft.Data.SqlClient.SqlConnection":
                    return DbStandardExpressions.SqlServer2012;
                case "Oracle.ManagedDataAccess.Client.OracleConnection":
                    return DbStandardExpressions.Oracle11;

                default:
                    throw new ArgumentException($"Connection type '{connection.GetType().Name}' not expected.", "connection");
            }
        }

        public static bool HasOracleProvider(this IDbConnection connection)
        {
            Condition.Requires(connection, nameof(connection)).IsNotNull();
            switch (connection.GetType().ToString())
            {
                case "Oracle.ManagedDataAccess.Client.OracleConnection":
                    return true;
                default:
                    return false;
            }
        }

        public static bool HasSqlServerProvider(this IDbConnection connection)
        {
            Condition.Requires(connection, nameof(connection)).IsNotNull();
            switch (connection.GetType().ToString())
            {
                case "System.Data.SqlClient.SqlConnection":
                case "Microsoft.Data.SqlClient.SqlConnection":
                    return true;
                default:
                    return false;
            }
        }
        public static IValueGenerator<Guid> SequentialGuidGenerator(this IDbConnection connection)
        {
            Condition.Requires(connection, nameof(connection)).IsNotNull();
            if (connection.HasSqlServerProvider()) return new SequentialSqlServerGuidGenerator();
            return new SequentialBinaryGuidGenerator();
        }

        #endregion Methods

    }
}