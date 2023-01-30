using System.Data.Common;
using System.Collections.Generic;
using System;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Domain;

    /// <remarks>
    /// Use dynamic type to avoid to add a dependency on the Oracle library.
    /// </remarks>
    internal class OracleToRepositoryExceptionTranslator : ObjectTranslator<DbException, RepositoryException>
    {
        #region Methods

        public override RepositoryException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Type entityType = null;
            context?.TryGetValue("EntityType", out entityType);
            var outerException = context.ContainsKey("OuterException") ? (Exception)context["OuterException"] : exception;
            dynamic oracleException = exception;
            foreach (dynamic error in oracleException.Errors)
            {
                if (OracleErrorHelper.IsUnavailableError(error))
                    return new RepositoryUnavailableException(entityType, outerException);

                if (OracleErrorHelper.IsUnauthorizedError(error))
                    return new RepositoryUnauthorizedException(entityType, outerException);

                if (OracleErrorHelper.IsTimeoutError(error))
                    return new RepositoryTimeoutException(entityType, outerException);

                if (OracleErrorHelper.IsConflictError(error))
                    return new RepositoryConflictException(entityType, outerException);
            }
            return new RepositoryException(isTransient: false, entityType, outerException);
        }

        #endregion Methods
    }
}
