using Conditions;
using System.Data;
using System.Data.Common;
using System;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Mapping;

    /// <summary>
    /// Base class for handling database commands.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="ICommandHandler{TCommand}" />
    public abstract class DbCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly IObjectTranslator<DbException, CommandException> exceptionTranslator = DbToCommandExceptionTranslator.Default;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbCommandHandler{TCommand}"/> class.
        /// </summary>
        /// <param name="connectionFactory">The database connection factory.</param>
        protected DbCommandHandler(IDbConnectionFactory connectionFactory)
        {
            Condition.Requires(connectionFactory, nameof(connectionFactory)).IsNotNull();
            this.ConnectionFactory = connectionFactory;
        }

        #endregion Constructors

        #region Properties

        protected IDbConnectionFactory ConnectionFactory { get; }

        #endregion Properties

        #region Methods

        public void Handle(TCommand command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            try
            {
                using (var connection = this.ConnectionFactory.CreateOpenConnection())
                {
                    this.Execute(command, connection);
                }
            }
            catch (DbException ex)
            {
                throw this.exceptionTranslator.Translate(ex, new { Command = command });
            }
        }

        protected abstract void Execute(TCommand command, IDbConnection connection);

        #endregion Methods

    }
}