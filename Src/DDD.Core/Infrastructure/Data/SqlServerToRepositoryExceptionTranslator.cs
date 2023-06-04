﻿using System.Data.Common;
using EnsureThat;
using System;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Domain;

    internal class SqlServerToRepositoryExceptionTranslator : ObjectTranslator<DbException, RepositoryException>
    {
        #region Methods

        public override RepositoryException Translate(DbException exception, IMappingContext context)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue("EntityType", out Type entityType);
            var outerException = context.ContainsKey("OuterException") ? (Exception)context["OuterException"] : exception;
            dynamic sqlServerException = exception;
            foreach (dynamic error in sqlServerException.Errors)
            {
                if (SqlServerErrorHelper.IsUnavailableError(error))
                    return new RepositoryUnavailableException(entityType, outerException);

                if (SqlServerErrorHelper.IsUnauthorizedError(error))
                    return new RepositoryUnauthorizedException(entityType, outerException);

                if (SqlServerErrorHelper.IsTimeoutError(error))
                    return new RepositoryTimeoutException(entityType, outerException);

                if (SqlServerErrorHelper.IsConflictError(error))
                    return new RepositoryConflictException(entityType, outerException);
            }
            return new RepositoryException(isTransient: false, entityType, outerException);
        }

        #endregion Methods
    }
}
