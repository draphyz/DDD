using System.Data.Common;
using System.Collections.Generic;
using Conditions;

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
            this.translators.Add("Oracle.DataAccess.Client.OracleException", new OracleToQueryExceptionTranslator());
        }

        #endregion Constructors

        #region Methods

        public override QueryException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            var exceptionType = exception.GetType().FullName;
            if (this.translators.TryGetValue(exceptionType, out var translator))
                return translator.Translate(exception, context);
            else
            {
                IQuery query = null;
                context?.TryGetValue("Query", out query);
                return new QueryException(isTransient: false, query, exception);
            }
        }

        #endregion Methods

    }
}