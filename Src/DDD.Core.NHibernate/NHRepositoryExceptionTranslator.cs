using System;
using NHibernate;
using NHibernate.Exceptions;
using System.Collections.Generic;
using System.Data.Common;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Core.Domain;

    internal class NHRepositoryExceptionTranslator : ObjectTranslator<Exception, RepositoryException>
    {

        #region Fields

        private readonly IObjectTranslator<DbException, RepositoryException> dbExceptionTranslator = new DbToRepositoryExceptionTranslator();

        #endregion Fields

        #region Methods

        public override RepositoryException Translate(Exception exception, IDictionary<string, object> context = null)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Type entityType = null;
            context?.TryGetValue("EntityType", out entityType);
            switch (exception)
            {
                case DbException dbEx:
                    return dbExceptionTranslator.Translate(dbEx, context);
                case ADOException _:
                    var dbException = ADOExceptionHelper.ExtractDbException(exception);
                    if (dbException != null)
                    {
                        context.Add("OuterException", exception);
                        return dbExceptionTranslator.Translate(dbException, context);
                    }
                    break;
            }
            return new RepositoryException(isTransient: false, entityType, exception);
        }

        #endregion Methods

    }
}
