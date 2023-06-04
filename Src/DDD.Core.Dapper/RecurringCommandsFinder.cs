using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;
    using Mapping;
    using Threading;

    public class RecurringCommandsFinder<TContext> : IQueryHandler<FindRecurringCommands, IEnumerable<RecurringCommand>, TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, QueryException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public RecurringCommandsFinder(IDbConnectionProvider<TContext> connectionProvider)
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

        public IEnumerable<RecurringCommand> Handle(FindRecurringCommands query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            try
            {
                var connection = this.connectionProvider.GetOpenConnection();
                var expressions = connection.Expressions();
                return connection.Query<RecurringCommand>
                (
                    new CommandDefinition
                    (
                        SqlScripts.FindRecurringCommands
                    )
                );
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        public async Task<IEnumerable<RecurringCommand>> HandleAsync(FindRecurringCommands query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                var cancellationToken = context.CancellationToken();
                var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                var expressions = connection.Expressions();
                return await connection.QueryAsync<RecurringCommand>
                (
                    new CommandDefinition
                    (
                        SqlScripts.FindRecurringCommands,
                        cancellationToken: cancellationToken
                    )
                );
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        #endregion Methods

    }
}
