using System.Data.Common;
using System.Collections.Generic;
using System;
using EnsureThat;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Collections;
    using Domain;

    public class DbToRepositoryExceptionTranslator : ObjectTranslator<DbException, RepositoryException>
    {

        #region Fields

        private readonly Dictionary<string, IObjectTranslator<DbException, RepositoryException>> translators = new Dictionary<string, IObjectTranslator<DbException, RepositoryException>>();

        #endregion Fields

        #region Constructors

        public DbToRepositoryExceptionTranslator()
        {
            this.translators.Add("System.Data.SqlClient.SqlException", new SqlServerToRepositoryExceptionTranslator());
            this.translators.Add("Microsoft.Data.SqlClient.SqlException", new SqlServerToRepositoryExceptionTranslator());
            this.translators.Add("Oracle.ManagedDataAccess.Client.OracleException", new OracleToRepositoryExceptionTranslator());
        }

        #endregion Constructors

        #region Methods

        public override RepositoryException Translate(DbException exception, IDictionary<string, object> context = null)
        {
            Ensure.That(exception, nameof(exception)).IsNotNull();
            var exceptionType = exception.GetType().FullName;
            if (this.translators.TryGetValue(exceptionType, out var translator))
                return translator.Translate(exception, context);
            else
            {
                Type entityType = null;
                context?.TryGetValue("EntityType", out entityType);
                var outerException = context.ContainsKey("OuterException") ? (Exception)context["OuterException"] : exception;
                return new RepositoryException(isTransient: false, entityType, outerException);
            }
        }

        #endregion Methods

    }
}