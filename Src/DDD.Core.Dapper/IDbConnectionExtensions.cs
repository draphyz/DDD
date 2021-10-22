using Dapper;
using Conditions;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DDD.Core.Infrastructure.Data
{
    /// <summary>
    /// Adds extension method to the <see cref="IDbConnection"/>  interface.
    /// </summary>
    public static class IDbConnectionExtensions
    {

        #region Methods

        public static int[] ExecuteScript(this IDbConnection connection,
                                          string script,
                                          IDbTransaction transaction = null,
                                          int? commandTimeout = null,
                                          string batchSeparator = "GO", 
                                          bool removeComments = true)
        {
            Condition.Requires(connection, nameof(connection)).IsNotNull();
            Condition.Requires(script, nameof(script)).IsNotNullOrEmpty();
            Condition.Requires(nameof(batchSeparator), batchSeparator).IsNotNullOrWhiteSpace();
            var scriptSplitter = new DbScriptSplitter();
            var commands = scriptSplitter.Split(script, batchSeparator, removeComments);
            var result = new List<int>();
            foreach(var command in commands)
            {
                if (StartsWithUseStatement(command))
                    connection.ExecuteUseStatement(command);
                result.Add(connection.Execute(command, null, transaction, commandTimeout));
            }
            return result.ToArray();
        }

        private static bool StartsWithUseStatement(string command)
        {
            return command.StartsWith("USE ", StringComparison.OrdinalIgnoreCase);
        }

        private static void ExecuteUseStatement(this IDbConnection connection, string command)
        {
            var buffer = command.Remove(0, 3).TrimStart();
            var databaseName = Regex.Split(buffer, @"[\s;]+")[0].TrimStart('[').TrimEnd(']');
            connection.ChangeDatabase(databaseName);
        }

        public static TValue NextValue<TValue>(this IDbConnection connection, string sequence, string schema = null)
        {
            Condition.Requires(connection, nameof(connection)).IsNotNull();
            Condition.Requires(sequence, nameof(sequence)).IsNotNullOrEmpty();
            var expressions = connection.Expressions();
            var sql = $"SELECT {expressions.NextValue(sequence, schema)} {expressions.FromDummy()}";
            return connection.QuerySingle<TValue>(sql);
        }

        public static Task<TValue> NextValueAsync<TValue>(this IDbConnection connection, string sequence, string schema = null, CancellationToken cancellationToken = default)
        {
            Condition.Requires(connection, nameof(connection)).IsNotNull();
            Condition.Requires(sequence, nameof(sequence)).IsNotNullOrEmpty();
            var expressions = connection.Expressions();
            var sql = $"SELECT {expressions.NextValue(sequence, schema)} {expressions.FromDummy()}";
            return connection.QuerySingleAsync<TValue>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        }

        #endregion Methods

    }
}
