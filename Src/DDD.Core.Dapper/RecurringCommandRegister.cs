using System;
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

    public class RecurringCommandRegister<TContext> : ICommandHandler<RegisterRecurringCommand, TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, CommandException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public RecurringCommandRegister(IDbConnectionProvider<TContext> connectionProvider)
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

        public void Handle(RegisterRecurringCommand command, IMessageContext context = null)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var connection = this.connectionProvider.GetOpenConnection();
                    var parameterPrefix = connection.Expressions().ParameterPrefix();
                    var parameters = ToParameters(command, connection);
                    var recurringCommand = connection.QuerySingleOrDefault<RecurringCommand>
                    (
                        new CommandDefinition
                        (
                            SqlScripts.FindRecurringCommandByType.Replace("@", parameterPrefix),
                            parameters
                        )
                    );
                    if (recurringCommand == null)
                    {
                        connection.Execute
                        (
                            new CommandDefinition
                            (
                                SqlScripts.InsertRecurringCommand.Replace("@", parameterPrefix),
                                parameters
                            )
                        );
                    }
                    else if (HasChanges(command, recurringCommand))
                    {
                        connection.Execute
                        (
                            new CommandDefinition
                            (
                                SqlScripts.UpdateRecurringCommand.Replace("@", parameterPrefix),
                                parameters
                            )
                        );
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<CommandException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        public async Task HandleAsync(RegisterRecurringCommand command, IMessageContext context = null)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                await new SynchronizationContextRemover();
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var cancellationToken = context?.CancellationToken() ?? default;
                    var connection = await this.connectionProvider.GetOpenConnectionAsync(cancellationToken);
                    var parameterPrefix = connection.Expressions().ParameterPrefix();
                    var parameters = ToParameters(command, connection);
                    var recurringCommand = await connection.QuerySingleOrDefaultAsync<RecurringCommand>
                    (
                        new CommandDefinition
                        (
                            SqlScripts.FindRecurringCommandByType.Replace("@", parameterPrefix),
                            parameters,
                            cancellationToken: cancellationToken
                        )
                    );
                    if (recurringCommand == null)
                    {
                        await connection.ExecuteAsync
                        (
                            new CommandDefinition
                            (
                                SqlScripts.InsertRecurringCommand.Replace("@", parameterPrefix),
                                parameters,
                                cancellationToken: cancellationToken
                            )
                        );
                    }
                    else if (HasChanges(command, recurringCommand))
                    {
                        await connection.ExecuteAsync
                        (
                            new CommandDefinition
                            (
                                SqlScripts.UpdateRecurringCommand.Replace("@", parameterPrefix),
                                parameters,
                                cancellationToken: cancellationToken
                            )
                        );
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<CommandException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        private static bool HasChanges(RegisterRecurringCommand command, RecurringCommand recurringCommand)
        {
            if (command.Body != recurringCommand.Body) return true;
            if (command.BodyFormat != recurringCommand.BodyFormat) return true;
            if (command.RecurringExpression != recurringCommand.RecurringExpression) return true;
            return false;
        }

        /// <remarks>Workaround for https://github.com/DapperLib/Dapper/issues/303 </remarks>
        private object ToParameters(RegisterRecurringCommand command, IDbConnection connection)
        {
            if (connection.HasOracleProvider())
                return new
                {
                    CommandId = command.CommandId.ToByteArray(),
                    command.CommandType,
                    command.Body,
                    command.BodyFormat,
                    command.RecurringExpression
                };
            return command;
        }

        #endregion Methods

    }
}
