using System.Data.SqlClient;
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
            var sqlServerException = (SqlException)exception;
            foreach (SqlError error in sqlServerException.Errors)
            {
                if (error.IsUnavailableError())
                    return new RepositoryUnavailableException(entityType, outerException);

                if (error.IsUnauthorizedError())
                    return new RepositoryUnauthorizedException(entityType, outerException);

                if (error.IsTimeoutError())
                    return new RepositoryTimeoutException(entityType, outerException);
            }
            return new RepositoryException(isTransient: false, entityType, outerException);
        }

        #endregion Methods
    }
}
