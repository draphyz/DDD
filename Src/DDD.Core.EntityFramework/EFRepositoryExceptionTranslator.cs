using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Core.Domain;

    internal class EFRepositoryExceptionTranslator : ObjectTranslator<Exception, RepositoryException>
    {

        #region Fields

        private readonly IObjectTranslator<DbException, RepositoryException> dbExceptionTranslator = new DbToRepositoryExceptionTranslator();

        #endregion Fields

        #region Methods

        public override RepositoryException Translate(Exception exception, IMappingContext context)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue("EntityType", out Type entityType);
            switch (exception)
            {
                case DbUpdateConcurrencyException _:
                    return new RepositoryConflictException(entityType, exception);
                case TimeoutException _:
                    return new RepositoryTimeoutException(entityType, exception);
                default:
                    var unwrappedException = UnwrapException(exception);
                    if (unwrappedException is DbException dbException)
                    {
                        context.Add("OuterException", exception);
                        return dbExceptionTranslator.Translate(dbException, context);
                    }
                    else
                    {
                        return new RepositoryException(isTransient: false, entityType, exception);
                    }
            }
        }

        private static Exception UnwrapException(Exception exception)
        {
            switch (exception)
            {
                case DbUpdateException dbUpdateException:
                    return UnwrapException(dbUpdateException.InnerException);
                default:
                    return exception;
            }
        }

        #endregion Methods

    }
}
