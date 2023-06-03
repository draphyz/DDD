using System.Data.Common;
using System.Collections.Generic;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Application;

    public class DbToQueryExceptionTranslator : ObjectTranslator<DbException, QueryException>
    {

        #region Fields

        private readonly Dictionary<string, IObjectTranslator<DbException, QueryException>> translators = new Dictionary<string, IObjectTranslator<DbException, QueryException>>();

        #endregion Fields

        #region Constructors

        public DbToQueryExceptionTranslator()
        {
            this.translators.Add("System.Data.SqlClient.SqlException", new SqlServerToQueryExceptionTranslator());
            this.translators.Add("Microsoft.Data.SqlClient.SqlException", new SqlServerToQueryExceptionTranslator());
            this.translators.Add("Oracle.ManagedDataAccess.Client.OracleException", new OracleToQueryExceptionTranslator());
        }

        #endregion Constructors

        #region Methods

        public override QueryException Translate(DbException exception, IMappingContext context)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            var exceptionType = exception.GetType().FullName;
            if (this.translators.TryGetValue(exceptionType, out var translator))
                return translator.Translate(exception, context);
            else
            {
                context.TryGetValue("Query", out IQuery query);
                return new QueryException(isTransient: false, query, exception);
            }
        }

        #endregion Methods

    }
}