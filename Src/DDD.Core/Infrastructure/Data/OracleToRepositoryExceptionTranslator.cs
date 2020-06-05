using System.Data.Common;
using System.Collections.Generic;
using System;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Domain;

    internal class OracleToRepositoryExceptionTranslator : IObjectTranslator<DbException, RepositoryException>
    {
        #region Methods

        public RepositoryException Translate(DbException exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("EntityType"));
            var entityType = (Type)options["EntityType"];
            var outerException = options.ContainsKey("OuterException") ? (Exception)options["OuterException"] : exception;
            dynamic oracleException = exception;
            foreach (dynamic error in oracleException.Errors)
            {
                if (OracleErrorHelper.IsUnavailableError(error))
                    return new RepositoryUnavailableException(entityType, outerException);

                if (OracleErrorHelper.IsUnauthorizedError(error))
                    return new RepositoryUnauthorizedException(entityType, outerException);

                if (OracleErrorHelper.IsTimeoutError(error))
                    return new RepositoryTimeoutException(entityType, outerException);
            }
            return new RepositoryException(isTransient: false, entityType, outerException);
        }

        #endregion Methods
    }
}
