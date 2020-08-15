using System;
using NHibernate;
using NHibernate.Exceptions;
using System.Collections.Generic;
using System.Data.Common;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Core.Domain;

    internal class NHibernateRepositoryExceptionTranslator : IObjectTranslator<Exception, RepositoryException>
    {

        #region Fields

        public static readonly IObjectTranslator<Exception, RepositoryException> Default = new NHibernateRepositoryExceptionTranslator();

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
                case DbException dbEx:
                    return dbExceptionTranslator.Translate(dbEx, options);
                case ADOException _:
                    var dbException = ADOExceptionHelper.ExtractDbException(exception);
                    if (dbException != null)
                    {
                        options.Add("OuterException", exception);
                        return dbExceptionTranslator.Translate(dbException, options);
                    }
                    break;
            }
            return new RepositoryException(isTransient: false, entityType, exception);
        }

        #endregion Methods

    }
}
