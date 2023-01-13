using Conditions;
using System;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Mapping;

    public class RecurringCommandIdGenerator<TContext> : ISyncQueryHandler<GenerateRecurringCommandId, Guid, TContext>
        where TContext : class, IBoundedContext
    {

        #region Fields

        private readonly IDbConnectionProvider<TContext> connectionProvider;
        private readonly CompositeTranslator<Exception, QueryException> exceptionTranslator;

        #endregion Fields

        #region Constructors

        public RecurringCommandIdGenerator(IDbConnectionProvider<TContext> connectionProvider)
        {
            Condition.Requires(connectionProvider, nameof(connectionProvider)).IsNotNull();
            this.connectionProvider = connectionProvider;
            this.exceptionTranslator = new CompositeTranslator<Exception, QueryException>();
            this.exceptionTranslator.Register(new DbToQueryExceptionTranslator());
            this.exceptionTranslator.RegisterFallback();
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.connectionProvider.Context;

        #endregion Properties

        #region Methods

        public Guid Handle(GenerateRecurringCommandId query, IMessageContext context = null)
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            try
            {
                var connection = this.connectionProvider.GetOpenConnection();
                return connection.SequentialGuidGenerator().Generate();
            }
            catch (Exception ex) when (ex.ShouldBeWrappedIn<QueryException>())
            {
                throw this.exceptionTranslator.Translate(ex, new { Query = query });
            }
        }

        #endregion Methods

    }
}
