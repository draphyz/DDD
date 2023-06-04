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

        public override RepositoryException Translate(Exception exception, IMappingContext context)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue("EntityType", out Type entityType);
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
