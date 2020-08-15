using System.Data.Common;
using System.Collections.Generic;
using Conditions;
using System;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Domain;

    internal class SqlServerToRepositoryExceptionTranslator : IObjectTranslator<DbException, RepositoryException>
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
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (SqlServerErrorHelper.IsUnavailableError(error))
                    return new RepositoryUnavailableException(entityType, outerException);

                if (SqlServerErrorHelper.IsUnauthorizedError(error))
                    return new RepositoryUnauthorizedException(entityType, outerException);

                if (SqlServerErrorHelper.IsTimeoutError(error))
                    return new RepositoryTimeoutException(entityType, outerException);
            }
            return new RepositoryException(isTransient: false, entityType, outerException);
        }

        #endregion Methods
    }
}
