using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Application;

    internal class SqlServerToQueryExceptionTranslator : IObjectTranslator<DbException, QueryException>
    {
        #region Methods

        public QueryException Translate(DbException exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("Query"));
            var query = (IQuery)options["Query"];
            var sqlServerException = (SqlException)exception;
            foreach (SqlError error in sqlServerException.Errors)
            {
                if (error.IsUnavailableError())
                    return new QueryUnavailableException(query, exception);

                if (error.IsUnauthorizedError())
                    return new QueryUnauthorizedException(query, exception);

                if (error.IsTimeoutError())
                    return new QueryTimeoutException(query, exception);
            }
            return new QueryException(isTransient: false, query, exception);
        }

        #endregion Methods
    }
}
