using System.Data.Common;
using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Application;

    public class DbToQueryExceptionTranslator : IObjectTranslator<DbException, QueryException>
    {

        #region Fields

        public static readonly IObjectTranslator<DbException, QueryException> Default = new DbToQueryExceptionTranslator();

        private readonly Dictionary<string, IObjectTranslator<DbException, QueryException>> translators = new Dictionary<string, IObjectTranslator<DbException, QueryException>>();

        #endregion Fields

        #region Constructors

        public DbToQueryExceptionTranslator()
        {
            this.translators.Add("System.Data.SqlClient.SqlException", new SqlServerToQueryExceptionTranslator());
            this.translators.Add("Microsoft.Data.SqlClient.SqlException", new SqlServerToQueryExceptionTranslator());
            this.translators.Add("Oracle.DataAccess.Client.OracleException", new OracleToQueryExceptionTranslator());
        }

        #endregion Constructors

        #region Methods

        public QueryException Translate(DbException exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("Query"));
            var exceptionType = exception.GetType().FullName;
            if (this.translators.TryGetValue(exceptionType, out var translator))
                return translator.Translate(exception, options);
            else 
            {
                var query = (IQuery)options["Query"];
                return new QueryException(isTransient: false, query, exception);
            }
        }

        #endregion Methods

    }
}
