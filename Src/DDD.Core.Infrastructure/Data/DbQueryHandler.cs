using Conditions;
using System.Data;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    /// <summary>
    /// Base class for handling database queries.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <seealso cref="IQueryHandler{TQuery, TResult}" />
    public abstract class DbQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryHandler{TQuery, TResult}"/> class.
        /// </summary>
        /// <param name="connectionFactory">The database connection factory.</param>
        protected DbQueryHandler(IDbConnectionFactory connectionFactory)
        {
            Condition.Requires(connectionFactory, nameof(connectionFactory)).IsNotNull();
            this.ConnectionFactory = connectionFactory;
        }

        #endregion Constructors

        #region Properties

        protected IDbConnectionFactory ConnectionFactory { get; }

        #endregion Properties

        #region Methods

        public TResult Handle(TQuery query)
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            using (var connection = this.ConnectionFactory.CreateConnection())
            {
                connection.Open();
                return this.Execute(query, connection);
            }
        }

        protected abstract TResult Execute(TQuery query, IDbConnection connection);

        #endregion Methods

    }
}