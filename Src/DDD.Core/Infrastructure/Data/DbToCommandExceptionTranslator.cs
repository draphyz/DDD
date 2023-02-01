using System.Data.Common;
using System.Collections.Generic;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Application;

    public class DbToCommandExceptionTranslator : ObjectTranslator<DbException, CommandException>
    {

        #region Fields

        private readonly Dictionary<string, IObjectTranslator<DbException, CommandException>> translators = new Dictionary<string, IObjectTranslator<DbException, CommandException>>();

        #endregion Fields

        #region Constructors

        public DbToCommandExceptionTranslator()
        {
            this.translators.Add("System.Data.SqlClient.SqlException", new SqlServerToCommandExceptionTranslator());
            this.translators.Add("Microsoft.Data.SqlClient.SqlException", new SqlServerToCommandExceptionTranslator());
            this.translators.Add("Oracle.ManagedDataAccess.Client.OracleException", new OracleToCommandExceptionTranslator());
        }

        #endregion Constructors

        #region Methods

        public override CommandException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            var exceptionType = exception.GetType().FullName;
            if (this.translators.TryGetValue(exceptionType, out var translator))
                return translator.Translate(exception, context);
            else
            {
                ICommand command = null;
                context?.TryGetValue("Command", out command);
                return new CommandException(isTransient: false, command, exception);
            }
        }

        #endregion Methods

    }
}