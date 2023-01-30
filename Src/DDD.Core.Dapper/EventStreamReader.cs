using System;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using DDD.Core.Domain;
    using Mapping;
    using Threading;

    public class EventStreamReader<TContext> : IQueryHandler<ReadEventStream, IEnumerable<Event>, TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, QueryException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public EventStreamReader(IDbConnectionProvider<TContext> connectionProvider)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
            this.connectionProvider = connectionProvider;
            this.exceptionTranslator = new CompositeTranslator<Exception, QueryException>();
            this.exceptionTranslator.Register(new DbToQueryExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.connectionProvider.Context;

        #endregion Properties

        #region Methods

        public IEnumerable<Event> Handle(ReadEventStream query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            try
            {
                var connection = this.connectionProvider.GetOpenConnection();
                return connection.Query<Event>
                (
                    new CommandDefinition
                    (
                        GetScript(connection),
                        ToParameters(query, connection)
                    )
                );
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        public async Task<IEnumerable<Event>> HandleAsync(ReadEventStream query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var cancellationToken = context?.CancellationToken() ?? default;
                var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                return await connection.QueryAsync<Event>
                (
                    new CommandDefinition
                    (
                        GetScript(connection),
                        ToParameters(query, connection),
                        cancellationToken: cancellationToken
                    )
                );
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        private static string GetScript(IDbConnection connection)
        {
            var expressions = connection.Expressions();
            var script = connection.HasOracleProvider() ? OracleScripts.ReadEventStream
                                                        : SqlScripts.ReadEventStream;
            return script.Replace("@", expressions.ParameterPrefix());
        }

        /// <remarks>Workaround for https://github.com/DapperLib/Dapper/issues/303 </remarks>
        private static object ToParameters(ReadEventStream query, IDbConnection connection)
        {
            if (!query.ExcludedStreamIds.Any())
                query.ExcludedStreamIds = new[] { " " }; // Workaround for https://github.com/DapperLib/Dapper/issues/202
            if (connection.HasOracleProvider())
                return new
                {
                    query.Top,
                    StreamPosition = query.StreamPosition.ToByteArray(),
                    query.ExcludedStreamIds,
                    query.StreamType
                };
            return query;
        }

        #endregion Methods

    }

}