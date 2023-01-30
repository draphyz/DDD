using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using DDD.Core.Domain;
    using Mapping;
    using Threading;

    public class EventStreamSubscriber<TContext> : ICommandHandler<SubscribeToEventStream, TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, CommandException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public EventStreamSubscriber(IDbConnectionProvider<TContext> connectionProvider)
        {
            Ensure.That(connectionProvider, nameof(connectionProvider)).IsNotNull();
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

        public void Handle(SubscribeToEventStream command, IMessageContext context = null)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
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
                            SqlScripts.SubscribeToEventStream.Replace("@", expressions.ParameterPrefix()),
                            ToParameters(command, connection)
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

        public async Task HandleAsync(SubscribeToEventStream command, IMessageContext context = null)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
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
                            SqlScripts.SubscribeToEventStream.Replace("@", expressions.ParameterPrefix()),
                            ToParameters(command, connection),
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
        private object ToParameters(SubscribeToEventStream command, IDbConnection connection)
        {
            if (connection.HasOracleProvider())
                return new
                {
                    command.Type,
                    command.Source,
                    Position = command.Position.ToByteArray(),
                    command.RetryMax,
                    command.RetryDelays,
                    command.BlockSize
                };
            return command;
        }

        #endregion Methods

    }
}
