using System.Data.Common;
using System.Collections.Generic;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Application;

    /// <remarks>
    /// Use dynamic type to avoid to add a dependency on the Oracle library.
    /// </remarks>
    internal class OracleToQueryExceptionTranslator : ObjectTranslator<DbException, QueryException>
    {
        #region Methods

        public override QueryException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            IQuery query = null;
            context?.TryGetValue("Query", out query);
            dynamic oracleException = exception;
            foreach (dynamic error in oracleException.Errors)
            {
                if (OracleErrorHelper.IsUnavailableError(error))
                    return new QueryUnavailableException(query, exception);

                if (OracleErrorHelper.IsUnauthorizedError(error))
                    return new QueryUnauthorizedException(query, exception);

                if (OracleErrorHelper.IsTimeoutError(error))
                    return new QueryTimeoutException(query, exception);

                if (OracleErrorHelper.IsConflictError(error))
                    return new QueryConflictException(query, exception);
            }
            return new QueryException(isTransient: false, query, exception);
        }

        #endregion Methods
    }
}
