using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;
    using Mapping;
    using Threading;

    public class SuccessfulRecurringCommandUpdater<TContext> : ICommandHandler<MarkRecurringCommandAsSuccessful, TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, CommandException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public SuccessfulRecurringCommandUpdater(IDbConnectionProvider<TContext> connectionProvider)
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

        public void Handle(MarkRecurringCommandAsSuccessful command, IMessageContext context)
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
                            SqlScripts.UpdateRecurringCommandStatus.Replace("@", expressions.ParameterPrefix()),
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

        public async Task HandleAsync(MarkRecurringCommandAsSuccessful command, IMessageContext context)
        {
            Ensure.That(command, nameof(command)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var cancellationToken = context.CancellationToken();
                    var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                    var expressions = connection.Expressions();
                    await connection.ExecuteAsync
                    (
                        new CommandDefinition
                        (
                            SqlScripts.UpdateRecurringCommandStatus.Replace("@", expressions.ParameterPrefix()),
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
        private object ToParameters(MarkRecurringCommandAsSuccessful command, IDbConnection connection)
        {
            if (connection.HasOracleProvider())
                return new
                {
                    LastExecutionTime = command.ExecutionTime,
                    LastExecutionStatus = "S",
                    LastExceptionInfo = (string)null,
                    CommandId = command.CommandId.ToByteArray()
                };
            return new
            {
                LastExecutionTime = command.ExecutionTime,
                LastExecutionStatus = "S",
                LastExceptionInfo = (string)null,
                CommandId = command.CommandId
            };
        }

        #endregion Methods
    }
}
