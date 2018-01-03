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
                    return new SqlServer2012Expressions();

                case "Oracle.ManagedDataAccess.Client.OracleConnection":
                    return new Oracle11Expressions();

                default:
                    throw new ArgumentException($"Connection type '{connection.GetType().Name}' not expected.", "connection");
            }
        }

        #endregion Methods
    }
}