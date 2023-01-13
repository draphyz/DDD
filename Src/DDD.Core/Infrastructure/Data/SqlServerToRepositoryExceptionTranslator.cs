using System.Data.Common;
using System.Collections.Generic;
using Conditions;
using System;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Domain;

    internal class SqlServerToRepositoryExceptionTranslator : ObjectTranslator<DbException, RepositoryException>
    {
        #region Methods

        public override RepositoryException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Type entityType = null;
            context?.TryGetValue("EntityType", out entityType);
            var outerException = context.ContainsKey("OuterException") ? (Exception)context["OuterException"] : exception;
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (error.IsUnavailableError())
                    return new RepositoryUnavailableException(entityType, outerException);

                if (error.IsUnauthorizedError())
                    return new RepositoryUnauthorizedException(entityType, outerException);

                if (error.IsTimeoutError())
                    return new RepositoryTimeoutException(entityType, outerException);

                if (error.IsConflictError())
                    return new RepositoryConflictException(entityType, outerException);
            }
            return new RepositoryException(isTransient: false, entityType, outerException);
        }

        #endregion Methods
    }
}
