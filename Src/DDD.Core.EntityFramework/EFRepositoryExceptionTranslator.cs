using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Core.Domain;

    internal class EFRepositoryExceptionTranslator : IObjectTranslator<Exception, RepositoryException>
    {

        #region Fields

        public static readonly IObjectTranslator<Exception, RepositoryException> Default = new EFRepositoryExceptionTranslator();

        private readonly IObjectTranslator<DbException, RepositoryException> dbExceptionTranslator = DbToRepositoryExceptionTranslator.Default;

        #endregion Fields

        #region Methods

        public RepositoryException Translate(Exception exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("EntityType"));
            var entityType = (Type)options["EntityType"];
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
                        options.Add("OuterException", exception);
                        return dbExceptionTranslator.Translate(dbException, options);
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
