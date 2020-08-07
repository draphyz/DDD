using System.Data.Common;
using System.Collections.Generic;
using System;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Mapping;
    using Domain;

    public class DbToRepositoryExceptionTranslator : IObjectTranslator<DbException, RepositoryException>
    {

        #region Fields

        public static readonly IObjectTranslator<DbException, RepositoryException> Default = new DbToRepositoryExceptionTranslator();

        private readonly Dictionary<string, IObjectTranslator<DbException, RepositoryException>> translators = new Dictionary<string, IObjectTranslator<DbException, RepositoryException>>();

        #endregion Fields

        #region Constructors

        public DbToRepositoryExceptionTranslator()
        {
            this.translators.Add("System.Data.SqlClient.SqlException", new SqlServerToRepositoryExceptionTranslator());
            this.translators.Add("Microsoft.Data.SqlClient.SqlException", new SqlServerToRepositoryExceptionTranslator());
            this.translators.Add("Oracle.DataAccess.Client.OracleException", new OracleToRepositoryExceptionTranslator());
        }

        #endregion Constructors

        #region Methods

        public RepositoryException Translate(DbException exception, IDictionary<string, object> options = null)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            Condition.Requires(options, nameof(options))
                     .IsNotNull()
                     .Evaluate(options.ContainsKey("EntityType"));
            var exceptionType = exception.GetType().FullName;
            if (this.translators.TryGetValue(exceptionType, out var translator))
                return translator.Translate(exception, options);
            else 
            {
                var entityType = (Type)options["EntityType"];
                var outerException = options.ContainsKey("OuterException") ? (Exception)options["OuterException"] : exception;
                return new RepositoryException(isTransient: false, entityType, outerException);
            }
        }

        #endregion Methods

    }
}
