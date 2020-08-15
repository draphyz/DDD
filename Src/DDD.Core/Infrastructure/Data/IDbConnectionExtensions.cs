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

        #endregion Methods
    }
}