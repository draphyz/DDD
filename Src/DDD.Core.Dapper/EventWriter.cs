using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using DDD.Core.Domain;
    using Mapping;
    using Threading;

    public class EventWriter<TContext> : ICommandHandler<WriteEvents, TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, CommandException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public EventWriter(IDbConnectionProvider<TContext> connectionProvider)
        {
            Condition.Requires(connectionProvider, nameof(connectionProvider)).IsNotNull();
            this.connectionProvider = connectionProvider;
            this.exceptionTranslator = new CompositeTranslator<Exception, CommandException>();
            this.exceptionTranslator.Register(new DbToCommandExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.connectionProvider.Context;

        #endregion Properties

        #region Methods

        public void Handle(WriteEvents command, IMessageContext context = null)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                using (var scope = new TransactionScope())
                {
                    var connection = this.connectionProvider.GetOpenConnection();
                    var expressions = connection.Expressions();
                    connection.Execute
                    (
                        new CommandDefinition
                        (
                            SqlScripts.WriteEvents.Replace("@", expressions.ParameterPrefix()),
                            ToParameter(command, connection)
                        )
                    );
                    scope.Complete();
                }
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<CommandException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        public async Task HandleAsync(WriteEvents command, IMessageContext context = null)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var cancellationToken = context?.CancellationToken() ?? default;
                    var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                    var expressions = connection.Expressions();
                    await connection.ExecuteAsync
                    (
                        new CommandDefinition
                        (
                            SqlScripts.WriteEvents.Replace("@", expressions.ParameterPrefix()),
                            ToParameter(command, connection),
                            cancellationToken: cancellationToken
                        )
                    );
                    scope.Complete();
                }
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<CommandException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        /// <remarks>Workaround for https://github.com/DapperLib/Dapper/issues/303 </remarks>
        private static IEnumerable<object> ToParameter(WriteEvents command, IDbConnection connectionProvider)
        {
            if (connectionProvider.HasOracleProvider())
            {
                foreach(var @event in command.Events)
                    yield return new
                    {
                        EventId = @event.EventId.ToByteArray(),
                        @event.EventType,
                        @event.OccurredOn,
                        @event.Body,
                        @event.BodyFormat,
                        @event.StreamId,
                        @event.StreamType,
                        @event.IssuedBy
                    };
            }
            else
            {
                foreach (var @event in command.Events)
                    yield return @event;
            }
        }

        #endregion Methods
    }
}
