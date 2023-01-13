using System;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Mapping;
    using Threading;

    public class FailedEventStreamDeleter<TContext> : ICommandHandler<IncludeFailedEventStream, TContext>
        where TContext : class, IBoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, CommandException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public FailedEventStreamDeleter(IDbConnectionProvider<TContext> connectionProvider)
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

        public void Handle(IncludeFailedEventStream command, IMessageContext context = null)
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
                            SqlScripts.IncludeFailedEventStream.Replace("@", expressions.ParameterPrefix()),
                            command
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

        public async Task HandleAsync(IncludeFailedEventStream command, IMessageContext context = null)
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
                            SqlScripts.IncludeFailedEventStream.Replace("@", expressions.ParameterPrefix()),
                            command,
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

        #endregion Methods

    }
}
