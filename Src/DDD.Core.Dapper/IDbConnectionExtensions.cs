using System.Data;
using Dapper;
using EnsureThat;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace DDD.Core.Infrastructure.Data
{
    using Threading;

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
                                          bool removeComments = false)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            Ensure.That(script, nameof(script)).IsNotNullOrEmpty();
            Ensure.That(nameof(batchSeparator), batchSeparator).IsNotNullOrWhiteSpace();
            var scriptSplitter = new DbScriptSplitter();
            var commands = scriptSplitter.Split(script, batchSeparator, removeComments);
            var result = new List<int>();
            foreach (var command in commands)
            {
                if (StartsWithUseStatement(command))
                    connection.ExecuteUseStatement(command);
                result.Add(connection.Execute(command, null, transaction, commandTimeout));
            }
            return result.ToArray();
        }

        public static async Task<int[]> ExecuteScriptAsync(this IDbConnection connection,
                                                           string script,
                                                           IDbTransaction transaction = null,
                                                           int? commandTimeout = null,
                                                           string batchSeparator = "GO",
                                                           bool removeComments = false,
                                                           CancellationToken cancellationToken = default)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            Ensure.That(script, nameof(script)).IsNotNullOrEmpty();
            Ensure.That(nameof(batchSeparator), batchSeparator).IsNotNullOrWhiteSpace();
            await new SynchronizationContextRemover();
            var scriptSplitter = new DbScriptSplitter();
            var commands = scriptSplitter.Split(script, batchSeparator, removeComments);
            var result = new List<int>();
            foreach (var command in commands)
            {
                if (StartsWithUseStatement(command))
                    connection.ExecuteUseStatement(command);
                var commandDefinition = new CommandDefinition(command, null, transaction, commandTimeout, cancellationToken: cancellationToken);
                result.Add(await connection.ExecuteAsync(commandDefinition));
            }
            return result.ToArray();
        }

        public static int NextId(this IDbConnection connection, string table, string primaryKey, int startRangeId = 0)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            Ensure.That(table, nameof(table)).IsNotNullOrEmpty();
            Ensure.That(primaryKey, nameof(primaryKey)).IsNotNullOrEmpty();
            var expressions = connection.Expressions();
            var sql = $"SELECT MAX({primaryKey}) FROM {table} WHERE {primaryKey} >= @StartRangeId".Replace("@", expressions.ParameterPrefix());
            var result = connection.QuerySingleOrDefault<int?>(sql, new { StartRangeId = startRangeId });
            if (!result.HasValue) return startRangeId + 1;
            return result.Value + 1;
        }

        public static async Task<int> NextIdAsync(this IDbConnection connection, string table, string primaryKey, int startRangeId = 0, CancellationToken cancellationToken = default)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            Ensure.That(table, nameof(table)).IsNotNullOrEmpty();
            Ensure.That(primaryKey, nameof(primaryKey)).IsNotNullOrEmpty();
            await new SynchronizationContextRemover();
            var expressions = connection.Expressions();
            var command = $"SELECT MAX({primaryKey}) FROM {table} WHERE {primaryKey} >= @StartRangeId".Replace("@", expressions.ParameterPrefix());
            var commandDefinition = new CommandDefinition(command, new { StartRangeId = startRangeId }, cancellationToken: cancellationToken);
            var result = await connection.QuerySingleOrDefaultAsync<int?>(commandDefinition);
            if (!result.HasValue) return startRangeId + 1;
            return result.Value + 1;
        }

        public static TValue NextValue<TValue>(this IDbConnection connection, string sequence, string schema = null)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            Ensure.That(sequence, nameof(sequence)).IsNotNullOrEmpty();
            var expressions = connection.Expressions();
            var command = $"SELECT {expressions.NextValue(sequence, schema)} {expressions.FromDummy()}";
            return connection.QuerySingle<TValue>(command);
        }

        public static async Task<TValue> NextValueAsync<TValue>(this IDbConnection connection, string sequence, string schema = null, CancellationToken cancellationToken = default)
        {
            Ensure.That(connection, nameof(connection)).IsNotNull();
            Ensure.That(sequence, nameof(sequence)).IsNotNullOrEmpty();
            await new SynchronizationContextRemover();
            var expressions = connection.Expressions();
            var command = $"SELECT {expressions.NextValue(sequence, schema)} {expressions.FromDummy()}";
            var commandDefinition = new CommandDefinition(command, cancellationToken: cancellationToken);
            return await connection.QuerySingleAsync<TValue>(commandDefinition); 
        }
        private static void ExecuteUseStatement(this IDbConnection connection, string command)
        {
            var buffer = command.Remove(0, 3).TrimStart();
            var databaseName = Regex.Split(buffer, @"[\s;]+")[0].TrimStart('[').TrimEnd(']');
            connection.ChangeDatabase(databaseName);
        }

        private static bool StartsWithUseStatement(string command)
        {
            return command.StartsWith("USE ", StringComparison.OrdinalIgnoreCase);
        }

        #endregion Methods

    }
}
