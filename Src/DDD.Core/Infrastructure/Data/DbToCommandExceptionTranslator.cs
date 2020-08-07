using System.Data.Common;
using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Application;

    public class DbToCommandExceptionTranslator : IObjectTranslator<DbException, CommandException>
    {

        #region Fields

        public static readonly IObjectTranslator<DbException, CommandException> Default = new DbToCommandExceptionTranslator();

        private readonly Dictionary<string, IObjectTranslator<DbException, CommandException>> translators = new Dictionary<string, IObjectTranslator<DbException, CommandException>>();

        #endregion Fields

        #region Constructors

        public DbToCommandExceptionTranslator()
        {
            this.translators.Add("System.Data.SqlClient.SqlException", new SqlServerToCommandExceptionTranslator());
            this.translators.Add("Microsoft.Data.SqlClient.SqlException", new SqlServerToCommandExceptionTranslator());
            this.translators.Add("Oracle.DataAccess.Client.OracleException", new OracleToCommandExceptionTranslator());
        }

        #endregion Constructors

        #region Methods

        public CommandException Translate(DbException exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("Command"));
            var exceptionType = exception.GetType().FullName;
            if (this.translators.TryGetValue(exceptionType, out var translator))
                return translator.Translate(exception, options);
            else
            {
                var command = (ICommand)options["Command"];
                return new CommandException(isTransient: false, command, exception);
            }
        }

        #endregion Methods

    }
}
