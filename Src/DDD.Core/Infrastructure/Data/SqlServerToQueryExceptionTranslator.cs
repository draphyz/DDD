using System.Data.Common;
using System.Collections.Generic;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Application;

    internal class SqlServerToQueryExceptionTranslator : ObjectTranslator<DbException, QueryException>
    {
        #region Methods

        public override QueryException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            IQuery query = null;
            context?.TryGetValue("Query", out query);
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (error.IsUnavailableError())
                    return new QueryUnavailableException(query, exception);

                if (error.IsUnauthorizedError())
                    return new QueryUnauthorizedException(query, exception);

                if (error.IsTimeoutError())
                    return new QueryTimeoutException(query, exception);

                if (error.IsConflictError())
                    return new QueryConflictException(query, exception);
            }
            return new QueryException(isTransient: false, query, exception);
        }

        #endregion Methods
    }
}
