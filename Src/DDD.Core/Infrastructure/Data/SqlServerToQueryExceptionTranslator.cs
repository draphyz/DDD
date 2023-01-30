using System.Data.Common;
using System.Collections.Generic;
using EnsureThat;

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
            Ensure.That(exception, nameof(exception)).IsNotNull();
            IQuery query = null;
            context?.TryGetValue("Query", out query);
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (SqlServerErrorHelper.IsUnavailableError(error))
                    return new QueryUnavailableException(query, exception);

                if (SqlServerErrorHelper.IsUnauthorizedError(error))
                    return new QueryUnauthorizedException(query, exception);

                if (SqlServerErrorHelper.IsTimeoutError(error))
                    return new QueryTimeoutException(query, exception);

                if (SqlServerErrorHelper.IsConflictError(error))
                    return new QueryConflictException(query, exception);
            }
            return new QueryException(isTransient: false, query, exception);
        }

        #endregion Methods
    }
}
