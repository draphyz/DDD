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
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (SqlServerErrorHelper.IsUnavailableError(error))
                    return new QueryUnavailableException(query, exception);

                if (SqlServerErrorHelper.IsUnauthorizedError(error))
                    return new QueryUnauthorizedException(query, exception);

                if (SqlServerErrorHelper.IsTimeoutError(error))
                    return new QueryTimeoutException(query, exception);
            }
            return new QueryException(isTransient: false, query, exception);
        }

        #endregion Methods
    }
}
